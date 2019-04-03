using System.Collections.Generic;

namespace MPK.Connect.Service.Business.AntColony
{
    public class Ant
    {
        public int CurrentNodeId { get; set; }
        public int Id { get; set; }
        public double PathCost { get; set; }
        public int Run { get; }
        public List<int> VisitedNodeIds { get; }

        public Ant(int id)
        {
            VisitedNodeIds = new List<int>();
            Run = 0;
            Id = id;
            PathCost = 0d;
        }

        public void Clear()
        {
            VisitedNodeIds.Clear();
            PathCost = 0d;
        }

        public void VisitNode(int nodeId, double cost = 0d)
        {
            VisitedNodeIds.Add(nodeId);
            PathCost += cost;
        }
    }
}