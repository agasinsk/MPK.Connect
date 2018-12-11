using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Service.Business.Graph;
using System;

namespace MPK.Connect.Service.Business
{
    public class TravelPlanService : ITravelPlanService
    {
        private readonly IGraphBuilder _graphBuilder;

        public TravelPlanService(IGraphBuilder graphBuilder)
        {
            _graphBuilder = graphBuilder ?? throw new ArgumentNullException(nameof(graphBuilder));
        }

        public string GetTravelPlan(Location sourceLocation, Location destinationLocation)
        {
            var graph = _graphBuilder.GetGraph();

            throw new System.NotImplementedException();
        }
    }
}