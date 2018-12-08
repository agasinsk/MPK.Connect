using System.Collections.Generic;

namespace MPK.Connect.Model.Graph
{
    public class StopTimeGraphNode
    {
        public StopTime StopTime { get; set; }

        public ICollection<StopTimeGraphEdge> Neighbors { get; set; }

        public StopTimeGraphNode()
        {
        }

        public StopTimeGraphNode(StopTime stopTime)
        {
            StopTime = stopTime;
            Neighbors = new List<StopTimeGraphEdge>();
        }

        public StopTimeGraphNode(StopTime stopTime, ICollection<StopTimeGraphEdge> neighbors)
        {
            StopTime = stopTime;
            Neighbors = neighbors;
        }
    }
}