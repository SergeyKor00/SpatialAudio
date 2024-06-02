using UnityEngine;

namespace Core.PathBuilding.Model
{
    public class EdgeInPath
    {
        public NodeInPath StartNode, EndNode;
        public float Distance;

        public EdgeInPath(NodeInPath startNode, NodeInPath endNode)
        {
            StartNode = startNode;
            EndNode = endNode;
            Distance = Vector2.Distance(StartNode.MyAnchor.Position, EndNode.MyAnchor.Position);
        }
    }
}