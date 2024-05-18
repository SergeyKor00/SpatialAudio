using System.Collections.Generic;
using System.Linq;
using Core.Engine.Interfaces;
using NUnit.Framework;
using SpatialAudio.Code;
using UnityEngine;

namespace SpatialAudio.Tests
{
    [TestFixture]
    public class PathFinderTest
    {

        private WideSearchPathFinder _wideSearchPathFinder;
        
        
        [Test]
        public void CreateSceneWithoutObstructs_CheckSinglePathToListener()
        {
            CreatePathFinder(new List<Segment>(), new List<Node>());

            var source = new TestAudioSource(Vector2.zero);
            var listener = new TestAudioSource(Vector2.one);
            var path = _wideSearchPathFinder.GetAllPathesToListener(source, listener).ToList();
            
            Assert.AreEqual(1, path.Count, "Checking length of path array");
            Assert.AreEqual(2, path[0].Points.Length);
            Assert.AreEqual(Vector2.zero, path[0].Points[0]);
            Assert.AreEqual(Vector2.one, path[0].Points[1]);
            
        }

        [Test]
        public void CreateSceneWithTriangleThatNotCrossingPath_CheckSinglePathToListener()
        {

            CreateTriangle();

            var source = new TestAudioSource(Vector2.zero);
            var listener = new TestAudioSource(Vector2.one);
            var path = _wideSearchPathFinder.GetAllPathesToListener(source, listener).ToList();
            
            Assert.AreEqual(1, path.Count, "Checking length of path array");
            Assert.AreEqual(2, path[0].Points.Length);
            Assert.AreEqual(Vector2.zero, path[0].Points[0]);
            Assert.AreEqual(Vector2.one, path[0].Points[1]);
            
        }
        
        [Test]
        public void CreateSceneWithTriangleThatCrossingPath_CheckPathCountToListener_EqualTwo()
        {

            CreateTriangle();

            var source = new TestAudioSource(Vector2.zero);
            var listener = new TestAudioSource(new Vector2(-1, 3));
            var path = _wideSearchPathFinder.GetAllPathesToListener(source, listener).ToList();
            
            Assert.AreEqual(2, path.Count, "Checking length of path array");
            
        }
        
        

        private void CreateTriangle()
        {
            var node1 = new Node(new Vector2(0, 1));
            var node2 = new Node(new Vector2(-1, 1));
            var node3 = new Node(new Vector2(0, 2));

            var segmentOne = new Segment(node1, node2, Vector2.down);
            var segmentTwo = new Segment(node2, node3, new Vector2(-1, 1).normalized);
            var segmentThree = new Segment(node3, node1, Vector2.right);

            node1.LeftSegment = segmentOne;
            node1.RightSegment = segmentThree;
            node2.LeftSegment = segmentTwo;
            node2.RightSegment = segmentOne;
            node3.LeftSegment = segmentThree;
            node3.RightSegment = segmentTwo;
            
            
            CreatePathFinder(new List<Segment>() {segmentOne, segmentTwo, segmentThree}, new List<Node>(){node1, node2, node3});
        }


        private void CreatePathFinder(List<Segment> segmentsOnScene, List<Node> nodesOnScene)
        {
            _wideSearchPathFinder = new WideSearchPathFinder(segmentsOnScene, nodesOnScene);
        }
        
        
       
        
        
        private class TestAudioSource : IAudioSource
        {
            public TestAudioSource(Vector2 position)
            {
                Position = position;
            }

            public Vector2 Position { get; }
        }
        
    }
}