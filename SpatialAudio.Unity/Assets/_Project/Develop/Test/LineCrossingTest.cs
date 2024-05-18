using System.Collections;
using NUnit.Framework;
using SpatialAudio.Code;
using UnityEngine;
using UnityEngine.TestTools;

namespace SpatialAudio.Tests
{
    public class LineCrossingTest
    {
        [Test]
        public void CheckTwoSegmentsAreCrossing()
        {
            var segmentOne = new Segment(new Vector2(-1, 0), new Vector2(1, 0));
            var segmentTwo =  new Segment(new Vector2(0, 1), new Vector2(0, -1));
            
            
            Assert.IsTrue(LineCrossingChecker.GetIntersectionPoint(segmentOne, segmentTwo, out var result));
        }

        [Test]
        public void TwoSegmentsAreCrossingCheckResultPoint()
        {
            var segmentOne = new Segment(new Vector2(-1, 0), new Vector2(1, 0));
            var segmentTwo =  new Segment(new Vector2(1, 1), new Vector2(1, -1));
            
            
            LineCrossingChecker.GetIntersectionPoint(segmentOne, segmentTwo, out var result);
            Assert.AreEqual(new Vector2(1, 0), result);
            
        }
        
        
        [Test]
        public void CheckTwoSegmentsAreNotCrossing()
        {
            var segmentOne = new Segment(new Vector2(-1, 0), new Vector2(0, 0));
            var segmentTwo =  new Segment(new Vector2(1, 1), new Vector2(1, -1));
            
            
            Assert.IsFalse(LineCrossingChecker.GetIntersectionPoint(segmentOne, segmentTwo, out var result));
           
        }
        
        
        [Test]
        public void CheckTwoParrallelSegmentsAreNotCrossing()
        {
            var segmentOne = new Segment(new Vector2(-1, 0), new Vector2(1, 0));
            var segmentTwo =  new Segment(new Vector2(-1, 1), new Vector2(1, 1));
            
            
            Assert.IsFalse(LineCrossingChecker.GetIntersectionPoint(segmentOne, segmentTwo, out var result));
           
        }
        
        
        [Test]
        public void CheckPointOnSegment()
        {
            var segmentOne = new Segment(new Vector2(-1, 1), new Vector2(1, -1));
            Assert.IsTrue(LineCrossingChecker.PointOnSegment(Vector2.zero, segmentOne));
           
        }
        
        [Test]
        public void PointOnSegment_WithPointInsideSegment_ReturnsTrue()
        {
            // Arrange
            Vector2 point = new Vector2(2, 3);
            Segment segment = new Segment(new Vector2(1, 1), new Vector2(4, 5));

            // Act
            bool result = LineCrossingChecker.PointOnSegment(point, segment);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void PointOnSegment_WithPointOutsideSegment_ReturnsFalse()
        {
            // Arrange
            Vector2 point = new Vector2(6, 6);
            Segment segment = new Segment(new Vector2(1, 1), new Vector2(4, 5));

            // Act
            bool result = LineCrossingChecker.PointOnSegment(point, segment);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void PointOnSegment_WithHorizontalSegment_ReturnsTrue()
        {
            // Arrange
            Vector2 point = new Vector2(3, 2);
            Segment segment = new Segment(new Vector2(1, 2), new Vector2(5, 2));

            // Act
            bool result = LineCrossingChecker.PointOnSegment(point, segment);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void PointOnSegment_WithVerticalSegment_ReturnsTrue()
        {
            // Arrange
            Vector2 point = new Vector2(4, 3);
            Segment segment = new Segment(new Vector2(4, 1), new Vector2(4, 5));

            // Act
            bool result = LineCrossingChecker.PointOnSegment(point, segment);

            // Assert
            Assert.IsTrue(result);
        }
        
        [Test]
        public void PointOnSegment_WithPointOnBorder_ReturnsTrue()
        {
            // Arrange
            Vector2 point = new Vector2(4, 3);
            Segment segment = new Segment(new Vector2(4, 3), new Vector2(4, 5));

            // Act
            bool result = LineCrossingChecker.PointOnSegment(point, segment);

            // Assert
            Assert.IsTrue(result);
        }
        
    }
}