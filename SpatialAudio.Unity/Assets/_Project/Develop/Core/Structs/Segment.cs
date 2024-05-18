using UnityEngine;

namespace SpatialAudio.Code
{
    public class Segment
    {
        public Vector2 Point1;
        public Vector2 Point2;

        public Node LeftNode, RightNode;

        public float Length;
        public Vector2 NormalVector;
        public bool SegmentBeenChecked;
        
        public Segment(Vector2 point1, Vector2 point2)
        {
            Point1 = point1;
            Point2 = point2;
        }

        public Segment(Node leftNode, Node rightNode, Vector2 normalVector) : this(leftNode.Position, rightNode.Position)
        {
            LeftNode = leftNode;
            RightNode = rightNode;
            
            Length = Vector2.Distance(leftNode.Position, rightNode.Position);
            NormalVector = normalVector;
        }
    }
}