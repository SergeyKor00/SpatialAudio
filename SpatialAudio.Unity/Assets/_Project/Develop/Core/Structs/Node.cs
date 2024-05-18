using UnityEngine;

namespace SpatialAudio.Code
{
    public class Node
    {
        public Vector2 Position;
        public Segment LeftSegment, RightSegment;
        public bool NodeBeenChecked;
        public string Name;
        public Node(Vector2 position)
        {
            Position = position;
        }
    }
}