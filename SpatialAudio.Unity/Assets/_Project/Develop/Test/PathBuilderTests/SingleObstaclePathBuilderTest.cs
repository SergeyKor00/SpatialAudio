using System.Linq;
using Core.PathBuilding._2dLocation.Logic;
using Core.PathBuilding._2dLocation.Structs;
using Core.PathBuilding.Interfaces;
using NUnit.Framework;
using UnityEngine;

namespace SpatialAudio.Tests.PathBuilderTests
{
    [TestFixture]
    public class SingleObstaclePathBuilderTest
    {
        private PathBuilder pathBuilder;
        private IVertexStorage vertexStorage;
        private LinesStorage linesStorage;

        private TestSoundAnchor source, listener;

        private void CreatePathBuilder()
        {
            var builder = new PathBuilder();
            builder.SetStorages(vertexStorage, linesStorage);
            
            builder.SetAudioSources(new ISoundAnchor[]{source});
            builder.SetSoundListener(listener);

            pathBuilder = builder;
        }

        [SetUp]
        public void PrepareForText()
        {
            vertexStorage = new SimpleVertexesStorage();
            linesStorage = new LinesStorage();

            source = new TestSoundAnchor();
            listener = new TestSoundAnchor();
            
            CreatePathBuilder();
            
            
            var vertex1 = new Vertex(new Vector2(0, 1));
            var vertex2 = new Vertex(new Vector2(-1, 1));
            var vertex3 = new Vertex(new Vector2(0, 2));

             var line1 = new Line(vertex1, vertex2, Vector2.down);
             var line2 = new Line(vertex2, vertex3, new Vector2(-1, 1).normalized);
             var line3 = new Line(vertex3, vertex1, Vector2.right);

             vertex1.LeftLine = line1;
             vertex1.RightLine = line3;

             vertex2.LeftLine = line2;
             vertex2.RightLine = line1;

             vertex3.LeftLine = line3;
             vertex3.RightLine = line2;

             vertexStorage.AddVertexAndReturnId(vertex1);
             vertexStorage.AddVertexAndReturnId(vertex2);
             vertexStorage.AddVertexAndReturnId(vertex3);

             linesStorage.AddLineAndReturnId(line1);
             linesStorage.AddLineAndReturnId(line2);
             linesStorage.AddLineAndReturnId(line3);
        }

        [Test]
        public void CreatePathBuilderCheckSoundGraphIsNotNull()
        {
            //Arrange
            source.Position = Vector2.zero;
            listener.Position = Vector2.one;
            
            //Act
            var pathGraph = pathBuilder.CreateGraph();
            
            
            //Assert
            Assert.IsNotNull(pathGraph);
        }


        [Test]
        public void SetListenerBeyondTriangleCheckListenerHasTwoEdges()
        {
            //Arrange
            source.Position = Vector2.zero;
            listener.Position = new Vector2(-1, 3);
            
            //Act
            var pathGraph = pathBuilder.CreateGraph();
            
            Assert.AreEqual(2, pathGraph.Listener.InnerEdges.Count);
        }
        
        
        [Test]
        public void SetListenerBeyondTriangleCheckSourceHasTwoEdges()
        {
            //Arrange
            source.Position = Vector2.zero;
            listener.Position = new Vector2(-1, 3);
            
            //Act
            var pathGraph = pathBuilder.CreateGraph();
            
            Assert.AreEqual(2, pathGraph.SoundSources[0].OuterEdges.Count);
        }


        [Test]
        public void SetListenerBeyondTriangleCheckLeftPathVertexPositions()
        {
            source.Position = Vector2.zero;
            listener.Position = new Vector2(-1, 3);
            
            //Act
            var pathGraph = pathBuilder.CreateGraph();
            var leftEdge = pathGraph.SoundSources[0].OuterEdges[1];
            Assert.AreEqual(vertexStorage.GetVertexById(1).Position, leftEdge.EndNode.MyAnchor.Position);
            Assert.AreEqual(pathGraph.SoundSources[0].MyAnchor.Position, leftEdge.StartNode.MyAnchor.Position);
            Assert.AreEqual(listener.Position, leftEdge.EndNode.OuterEdges[0].EndNode.MyAnchor.Position);
        }
        
        [Test]
        public void SetListenerBeyondTriangleCheckLeftPathEdgesAndNodesInContainer()
        {
            source.Position = Vector2.zero;
            listener.Position = new Vector2(-1, 3);
            
            //Act
            var pathGraph = pathBuilder.CreateGraph();
            var leftEdge = pathGraph.SoundSources[0].OuterEdges[1];
            
            Assert.AreEqual(1, leftEdge.EndNode.InnerEdges.Count);
            Assert.AreEqual(1, leftEdge.EndNode.OuterEdges.Count);
            
            
        }
        
        [Test]
        public void SetSourceBeyondTriangleCheckLeftPathEdgesAndNodesInContainer()
        {
            source.Position = new Vector2(-1, 3);
            listener.Position = Vector2.zero;
            
            //Act
            var pathGraph = pathBuilder.CreateGraph();
            var leftEdge = pathGraph.SoundSources[0].OuterEdges[0];
            
           
            Assert.AreEqual(vertexStorage.GetVertexById(1).Position, leftEdge.EndNode.MyAnchor.Position);
            Assert.AreEqual(pathGraph.SoundSources[0].MyAnchor.Position, leftEdge.StartNode.MyAnchor.Position);
            Assert.AreEqual(listener.Position, leftEdge.EndNode.OuterEdges[0].EndNode.MyAnchor.Position);
        }
        
        
    }
}