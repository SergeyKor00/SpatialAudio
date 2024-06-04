using System.Collections.Generic;
using System.Linq;
using Core.PathBuilding._2dLocation.Structs;
using Core.PathBuilding.Interfaces;
using Core.PathBuilding.Model;
using UnityEngine;

namespace Core.PathBuilding._2dLocation.Logic
{
    public class PathBuilder : IPathBuilder
    {
        private IVertexStorage _vertexStorage;
        private LinesStorage _linesStorage;

        private SoundPathGraph _pathGraph;

        private List<ISoundAnchor> _soundSources;
        private ISoundAnchor _soundListener;
        
        public void SetStorages(IVertexStorage vertexStorage, LinesStorage linesStorage)
        {
            _vertexStorage = vertexStorage;
            _linesStorage = linesStorage;
        }
        
        
        public void SetAudioSources(ISoundAnchor[] soundAnchors)
        {
            _soundSources = soundAnchors.ToList();
        }

        public void SetSoundListener(ISoundAnchor soundAnchor)
        {
            _soundListener = soundAnchor;
        }
        
        
        
        public SoundPathGraph CreateGraph()
        {
            _pathGraph = new SoundPathGraph();
            _pathGraph.Listener = new NodeInPath();
            _pathGraph.Listener.MyAnchor = _soundListener;


            foreach (var source in _soundSources)   
            {
                FindPathFromSource(source);
            }
            
            
            return _pathGraph;
        }

      
        /// <summary>
        /// Начать построение пути от источника звука к слушателю
        /// </summary>
        /// <param name="source">Источник звука</param>
        private void FindPathFromSource(ISoundAnchor source)
        {
            var node = new NodeInPath();
            node.MyAnchor = source;
            _pathGraph.SoundSources.Add(node);
            FindPathBetweenNodes(new ObstaclePathGraph(node, _pathGraph.Listener, new List<int>()));
            
        }


        /// <summary>
        /// Найти путь между 2умя узлами графа
        /// </summary>
        /// <param name="pathGraph"></param>
        private void FindPathBetweenNodes(ObstaclePathGraph pathGraph)
        {
            if (FindClosestLineIfFound(new Segment(pathGraph.StartNode.MyAnchor.Position, pathGraph.EndNode.MyAnchor.Position),
                     pathGraph.CheckedLines, out var crossData))
            {
                pathGraph.CheckedLines.Add(crossData.CrossedLine.Id);
                FindPathFromVertex(crossData.CrossedLine.LeftVertex, pathGraph.StartNode, crossData.CrossedLine, pathGraph);
                FindPathFromVertex(crossData.CrossedLine.RightVertex,pathGraph.StartNode, crossData.CrossedLine, pathGraph); 
                
            }
            else
            {
                ConnectEdges(pathGraph.StartNode, pathGraph.EndNode);
            }
        }

       
        private NodeInPath CreateNodeAtVertex(Vertex vertex, NodeInPath previewNode, ObstaclePathGraph pathGraph)
        {
          
            var newNode = new NodeInPath();
            newNode.MyAnchor = vertex;
            _pathGraph.Nodes.Add(vertex.Id, newNode);
            
            ConnectEdges(previewNode, newNode);
            
            return newNode;
        }
        
        
        /// <summary>
        /// Соединяет 2 узла графа между собой
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="endNode"></param>
        private void ConnectEdges(NodeInPath startNode, NodeInPath endNode)
        {
            var connectionEdge = new EdgeInPath(startNode, endNode);
            startNode.OuterEdges.Add(connectionEdge);
            endNode.InnerEdges.Add(connectionEdge);
            
        }

        private void OptimizePathFromPreviewNodes(NodeInPath nodeToAvoid, NodeInPath currNode, Line connectionLine)
        {
            if(nodeToAvoid.InnerEdges.Count == 0)
                return;

            
            var nodesToCheck = nodeToAvoid.InnerEdges.Select(e => e.StartNode);
            foreach (var node in nodesToCheck)
            {
                if (Vector2.Angle(node.MyAnchor.Position - currNode.MyAnchor.Position, connectionLine.NormalVector) >= 90.0f)
                {
                    continue;
                }

                var segmentToTarget = new Segment(node.MyAnchor.Position, currNode.MyAnchor.Position);
                var linesToAvoid = new List<int>();
                var currVertex = currNode.MyAnchor as Vertex;
                
                linesToAvoid.Add(currVertex.RightLine.Id);
                linesToAvoid.Add(currVertex.LeftLine.Id);

                if (node.InnerEdges.Count != 0)
                {
                    linesToAvoid.Add((node.MyAnchor as Vertex).RightLine.Id);
                    linesToAvoid.Add((node.MyAnchor as Vertex).LeftLine.Id);
                }

                if (CheckSegmentHasInterseptions(segmentToTarget, linesToAvoid))
                {
                    continue;
                }
                
                ReplaceNode(currNode, nodeToAvoid, node);
                
               
            }
        }

        private bool CheckSegmentHasInterseptions(Segment segmentToTarget, List<int> linesToAvoid)
        {
            
            
            foreach (var l in GetLinesToCheck(segmentToTarget, linesToAvoid))
            {
                if (!LineCrossingChecker.GetIntersectionPointIfExists(segmentToTarget, l.GetSegment,
                        out var result))
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// найти точку пересечения отрезка с препятствиями
        /// </summary>
        /// <param name="segmentToTarget">Отрезок от начала к концу точки</param>
        /// <param name="linesToAvoid">id линий, которые не надо проверять</param>
        /// <param name="crossData">Ближайшая к началу отрезка точка переченения</param>
        /// <returns>Было ли найдено пересечение с препятсвиями</returns>
        private bool FindClosestLineIfFound(Segment segmentToTarget, List<int> linesToAvoid, out CrossData crossData)
        {
            CrossData closestCrossData = new CrossData();
            float closestCrossDistance = float.MaxValue;
            bool crossFound = false;
            
            foreach (var line in GetLinesToCheck(segmentToTarget, linesToAvoid))
            {
                
                if (LineCrossingChecker.GetIntersectionPointIfExists(segmentToTarget, line.GetSegment, out var result))
                {
                    crossFound = true;
                    var lineToCrossPoint = segmentToTarget.Point1 - result;
                    if (DistanceLowerThenValue(lineToCrossPoint, closestCrossDistance))
                    {
                        closestCrossData.CrossPoint = result;
                        closestCrossData.CrossedLine = line;
                        closestCrossDistance = lineToCrossPoint.sqrMagnitude;
                    }
                }
            }

            crossData = closestCrossData;
            return crossFound;
        }

        /// <summary>
        /// Проверить возможность построить путь из текущей точки
        /// </summary>
        /// <param name="previewNode"></param>
        /// <param name="previewLine">Линия, вдоль которой двигался алгоритм обхода препятствия</param>
        /// <param name="pathGraph"></param>
        /// <param name="vertex"></param>
        private void FindPathFromVertex(Vertex vertex, NodeInPath previewNode, Line previewLine, ObstaclePathGraph pathGraph)
        {
            if (_pathGraph.Nodes.TryGetValue(vertex.Id, out var node))
            {
                ConnectEdges(previewNode, node);
                OptimizePathFromPreviewNodes(previewNode, node, previewLine);
                return;
            }

            var nodeAtVertex = CreateNodeAtVertex(vertex, previewNode, pathGraph);
            OptimizePathFromPreviewNodes(previewNode, nodeAtVertex, previewLine);
            //var vertex = nodeWithVertex.MyAnchor as Vertex;
            var nextLine = vertex.LeftLine == previewLine ? vertex.RightLine : vertex.LeftLine;
            
            if (Vector2.Angle( pathGraph.EndNode.MyAnchor.Position - vertex.Position, nextLine.NormalVector) < 90.0f)
            {
                FindPathBetweenNodes(new ObstaclePathGraph(nodeAtVertex, pathGraph.EndNode, new List<int>(new [] {previewLine.Id, nextLine.Id})));
            }
            else
            {
                if (pathGraph.CheckedLines.Contains(nextLine.Id))
                {
                    return;
                }
                
                var nextVertex = nextLine.RightVertex == vertex ? nextLine.LeftVertex : nextLine.RightVertex;
                
                FindPathFromVertex(nextVertex, nodeAtVertex, nextLine, pathGraph);
            }
            

        }

        
        
        


        private IEnumerable<Line> GetLinesToCheck(Segment segmentToTarget, List<int> linesToAvoid)
        {
           return _linesStorage.GetAllLines().Where(l => !linesToAvoid.Contains(l.Id));
        }

        private void ReplaceNode(NodeInPath currNode, NodeInPath nodeToRemove, NodeInPath nodeToAdd)
        {
            ConnectEdges(nodeToAdd, currNode);
            nodeToAdd.RemoveEdgeWithNode(nodeToRemove);
            currNode.RemoveEdgeWithNode(nodeToRemove);
            
            nodeToRemove.RemoveEdgeWithNode(nodeToAdd);
            nodeToRemove.RemoveEdgeWithNode(currNode);

            if (nodeToRemove.InnerEdges.Count == 0 && nodeToRemove.OuterEdges.Count == 0)
            {
                _pathGraph.Nodes.Remove((nodeToRemove.MyAnchor as Vertex).Id);
            }
        }
        
        
        
        public void UpdateListener()
        {
            
        }

        public void UpdateSource()
        {
            
        }


        private  bool DistanceLowerThenValue( Vector2 vector, float value)
        {
            return vector.sqrMagnitude < value * value;
        }
        
        private struct CrossData
        {
            public Vector2 CrossPoint;
            public Line CrossedLine;
        }

        private class ObstaclePathGraph
        {
            public NodeInPath StartNode, EndNode;
            public List<int> CheckedLines;

            public ObstaclePathGraph(NodeInPath startNode, NodeInPath endNode, List<int> checkedLines)
            {
                StartNode = startNode;
                EndNode = endNode;
                CheckedLines = checkedLines;
            }
            
            
        }
        
    }
}