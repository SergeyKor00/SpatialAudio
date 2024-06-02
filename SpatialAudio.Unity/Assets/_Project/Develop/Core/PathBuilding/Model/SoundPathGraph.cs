using System.Collections.Generic;

namespace Core.PathBuilding.Model
{
    public class SoundPathGraph
    {
        public Dictionary<int, NodeInPath> Nodes = new Dictionary<int, NodeInPath>();
        public List<NodeInPath> SoundSources = new List<NodeInPath>();

        public NodeInPath Listener;
        
        
        
    }
}