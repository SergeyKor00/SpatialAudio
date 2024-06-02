using Core.PathBuilding._2dLocation.Logic;
using Core.PathBuilding.Interfaces;
using NUnit.Framework;
using UnityEngine;

namespace SpatialAudio.Tests.PathBuilderTests
{
    [TestFixture]
    public class NoObstaclesPathBuilderTest
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
        public void CreatePathBuilderCheckSoundGraphConstainsConnection()
        {
            
            //Arrange
            source.Position = Vector2.zero;
            listener.Position = Vector2.one;
            
            //Act
            var pathGraph = pathBuilder.CreateGraph();

            var sourceNode = pathGraph.SoundSources[0];
            var listenerNode = pathGraph.Listener;
            
            //Assert
            Assert.AreEqual(1, sourceNode.OuterEdges.Count);
            Assert.AreEqual(0, sourceNode.InnerEdges.Count);
            
            Assert.AreEqual(0, listenerNode.OuterEdges.Count);
            Assert.AreEqual(1, listenerNode.InnerEdges.Count);
            
            Assert.Contains(sourceNode.OuterEdges[0], listenerNode.InnerEdges);
            Assert.Contains(listenerNode.InnerEdges[0], sourceNode.OuterEdges);
            
        }


        [Test]
        public void CreatePathGraphCheckEdgeSize()
        {
            //Arrange
            source.Position = Vector2.zero;
            listener.Position = Vector2.one;
            
            //Act
            var pathGraph = pathBuilder.CreateGraph();

            var sourceNode = pathGraph.SoundSources[0];
            
            Assert.AreEqual(Vector2.Distance(Vector2.zero, Vector2.one), sourceNode.OuterEdges[0].Distance);
        }
    }
}