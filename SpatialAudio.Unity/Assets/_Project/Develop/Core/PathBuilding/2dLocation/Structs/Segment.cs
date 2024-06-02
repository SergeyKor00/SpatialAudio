using UnityEngine;

namespace Core.PathBuilding._2dLocation.Structs
{
    /// <summary>
    /// Отрезок в 2д пространстве
    /// </summary>
    public struct Segment
    {
        public Vector2 Point1;
        public Vector2 Point2;

        public Segment(Vector2 point1, Vector2 point2)
        {
            Point1 = point1;
            Point2 = point2;
        }
    }
}