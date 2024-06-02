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
    }
}