using System.Collections.Generic;
using System.Linq;
using Core.Engine.Interfaces;
using UnityEngine;

namespace SpatialAudio.Code
{
    public class WideSearchPathFinder
    {
        private List<Segment> _segmentsOnScene;
        private List<Node> _nodesOnScene;

        private Queue<WaysToCheck> _waysToListener;
        private List<SoundPath> _soundPaths;
        private IAudioSource _source, _listener;
        public WideSearchPathFinder(List<Segment> segmentsOnScene, List<Node> nodesOnScene)
        {
            _segmentsOnScene = segmentsOnScene;
            _nodesOnScene = nodesOnScene;
        }

        public IEnumerable<SoundPath> GetAllPathesToListener(IAudioSource source, IAudioSource listener)
        {
            _soundPaths = new List<SoundPath>();
            _waysToListener = new Queue<WaysToCheck>();
            _source = source;
            _listener = listener;

            ResetCollections();
            
            if (CheckLineHasAnyClossed(_source.Position, _listener.Position, out var crossData))
            {
                crossData.CrossedSegment.SegmentBeenChecked = true;
                
                _waysToListener.Enqueue(new WaysToCheck(_source.Position,crossData.CrossedSegment.LeftNode));
                _waysToListener.Enqueue(new WaysToCheck(_source.Position,crossData.CrossedSegment.RightNode));
                
            }
            else
            {
                _soundPaths.Add(new SoundPath(){Points = new  []{_source.Position, _listener.Position}});
            }
            

            while (_waysToListener.Count > 0)
            {
                var nextWay = _waysToListener.Dequeue();
                FindWayFromPoint(nextWay);
            }

            return _soundPaths;
        }

        private void ResetCollections()
        {
            foreach (var segment in _segmentsOnScene)
            {
                segment.SegmentBeenChecked = false;
                
            }

            foreach (var node in _nodesOnScene)
            {
                node.NodeBeenChecked = false;
            }
        }
        private void CheckWayToListener(Vector2 startPoint, WaysToCheck originWay)
        {
            
            if (CheckLineHasAnyClossed(startPoint, _listener.Position, out var crossData))
            {
                crossData.CrossedSegment.SegmentBeenChecked = true;
                
                _waysToListener.Enqueue(new WaysToCheck(originWay, crossData.CrossedSegment.LeftNode));
                _waysToListener.Enqueue(new WaysToCheck(originWay, crossData.CrossedSegment.RightNode));
            }
            else
            {
                var path = new SoundPath();
                var allPoint = originWay.GetAllPoint();
                allPoint.Add(_listener.Position);
                path.Points = allPoint.ToArray();
                    
                _soundPaths.Add(path);
            }
        }


        private void FindWayFromPoint(WaysToCheck way)
        {
            var lastNode = way.NodesInPath.Last();


            if (lastNode.LeftSegment.SegmentBeenChecked && lastNode.RightSegment.SegmentBeenChecked)
            {
                CheckWayToListener(lastNode.Position, way);
                return;
            }
                

            var nextSegment = lastNode.LeftSegment.SegmentBeenChecked ? lastNode.RightSegment : lastNode.LeftSegment;

            nextSegment.SegmentBeenChecked = true;
            if (Vector2.Angle(nextSegment.NormalVector, _listener.Position - lastNode.Position) >= 90.0f)
            {
                way.NodesInPath.AddLast(nextSegment.LeftNode != lastNode
                    ? nextSegment.LeftNode
                    : nextSegment.RightNode);
                _waysToListener.Enqueue(way);
            }
            else
            {
                CheckWayToListener(lastNode.Position, way);
            }
            lastNode.NodeBeenChecked = true;
        }
        
        
        
        private bool CheckLineHasAnyClossed(Vector2 origin, Vector2 target, out CrossData crossData)
        {
            var segmentToCompare = new Segment(origin, target);

            List<CrossData> crossesBeenFound = new List<CrossData>();
            
            foreach (var s in _segmentsOnScene)
            {
                if(s.SegmentBeenChecked)
                    continue;
                
                if (LineCrossingChecker.GetIntersectionPoint(segmentToCompare, s, out var point))
                {
                    crossesBeenFound.Add(new CrossData(){CrossedSegment = s, CrossPoint = point});
                }
            }

            if (crossesBeenFound.Count == 0)
            {
                crossData = new CrossData();
                return false;
            }
            else
            {
                crossesBeenFound.Sort((point1, point2) =>
                {
                    return Vector2.SqrMagnitude(point1.CrossPoint - origin).CompareTo(Vector2.SqrMagnitude(point2.CrossPoint - origin));
                } );

                crossData = crossesBeenFound[0];
                return true;
            }
            
            
        }
        
        
        private class WaysToCheck
        {
            public float CommonLength;
            public Vector2 Origin;
            public LinkedList<Node> NodesInPath;
            public WaysToCheck PreviousWay;
            
            
            public WaysToCheck(Vector2 origin, Node nodeToAdd)
            {
                Origin = origin;
                NodesInPath = new LinkedList<Node>();
                NodesInPath.AddFirst(nodeToAdd);
                nodeToAdd.NodeBeenChecked = true;
                CommonLength = Vector2.Distance(Origin, nodeToAdd.Position);
            }
            
            public WaysToCheck(WaysToCheck previousWay, Node nodeToAdd)
            {
                PreviousWay = previousWay;
                NodesInPath = new LinkedList<Node>();
                NodesInPath.AddFirst(nodeToAdd);
                nodeToAdd.NodeBeenChecked = true;
               
            }

            public List<Vector2> GetAllPoint()
            {
                var resultList = new List<Vector2>();
                if (PreviousWay != null)
                {
                    resultList.AddRange(PreviousWay.GetAllPoint());
                }
                else
                {
                    resultList.Add(Origin);
                }
                resultList.AddRange(NodesInPath.Select(n => n.Position));
                
                return resultList;
            }
            
        }
        
        private struct CrossData
        {
            public Segment CrossedSegment;
            public Vector2 CrossPoint;
        }
    }
}