using UnityEngine;

namespace Core.PathBuilding._2dLocation.Structs
{
    /// <summary>
    /// Класс, определяющий линию - границу препятствия в 2д пространстве
    /// </summary>
    public class Line
    {
        public int Id;

        public Vertex LeftVertex, RightVertex;
        public float Length;
        public Vector2 NormalVector;

        public Segment GetSegment => new Segment(LeftVertex.Position, RightVertex.Position);

        public Line()
        {
            
        }

        public Line(Vertex leftVertex, Vertex rightVertex, Vector2 normal)
        {
            LeftVertex = leftVertex;
            RightVertex = rightVertex;
            NormalVector = normal;
            Length = Vector2.Distance(leftVertex.Position, rightVertex.Position);
        }
    }
}