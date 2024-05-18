using UnityEngine;

namespace SpatialAudio.Code
{
    public class Segment
    {
        public Vector2 Point1, Point2;

        public Segment(Vector2 point1, Vector2 point2)
        {
            Point1 = point1;
            Point2 = point2;
        }
    }
}