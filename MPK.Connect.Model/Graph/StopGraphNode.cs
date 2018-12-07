using System.Collections.Generic;

namespace MPK.Connect.Model.Graph
{
    public class StopGraphNode
    {
        public ICollection<StopGraphEdge> Neighbors { get; set; }
        public Stop Stop { get; set; }

        public StopGraphNode()
        {
        }

        public StopGraphNode(Stop stop)
        {
            Stop = stop;
            Neighbors = new List<StopGraphEdge>();
        }

        public StopGraphNode(Stop stop, ICollection<StopGraphEdge> neighbors)
        {
            Stop = stop;
            Neighbors = neighbors;
        }
    }
}