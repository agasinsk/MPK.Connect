using System.Collections.Generic;

namespace MPK.Connect.Model.Graph
{
    public class StopGraph : Dictionary<string, StopGraphNode>
    {
        public Dictionary<string, StopGraphNode> Nodes { get; set; }

        public StopGraph()
        {
            Nodes = new Dictionary<string, StopGraphNode>();
        }

        public StopGraph(Dictionary<string, StopGraphNode> stops)
        {
            Nodes = stops;
        }
    }
}