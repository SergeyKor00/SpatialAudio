using System.Collections.Generic;
using Core.PathBuilding._2dLocation.Structs;
using Core.PathBuilding.Interfaces;
using UnityEngine;

namespace Core.PathBuilding.Model
{
    public class NodeInPath
    {
        public ISoundAnchor MyAnchor;
        public List<EdgeInPath> InnerEdges = new List<EdgeInPath>();
        public List<EdgeInPath> OuterEdges = new List<EdgeInPath>();

        public void RemoveEdgeWithNode(NodeInPath node)
        {
            for (int i = 0; i < OuterEdges.Count; i++)
            {
                if (OuterEdges[i].EndNode == node)
                {
                    OuterEdges.RemoveAt(i);
                    break;
                }
            }
            
            for (int i = 0; i < InnerEdges.Count; i++)
            {
                if (InnerEdges[i].StartNode == node)
                {
                    InnerEdges.RemoveAt(i);
                    break;
                }
            }
        }
        
    }
}