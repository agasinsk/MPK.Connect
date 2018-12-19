using System;
using System.Collections.Generic;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Service.Business.Graph;

namespace MPK.Connect.Service.Business
{
    public class TravelPlanService : ITravelPlanService
    {
        private readonly IGraphBuilder _graphBuilder;
        private readonly ITravelPlanProvider _travelPlanProvider;
        private readonly ICoordinateLimitsProvider _coordinateLimitsProvider;

        public TravelPlanService(IGraphBuilder graphBuilder, ICoordinateLimitsProvider coordinateLimitsProvider, ITravelPlanProvider travelPlanProvider)
        {
            _coordinateLimitsProvider = coordinateLimitsProvider ?? throw new ArgumentNullException(nameof(coordinateLimitsProvider));
            _graphBuilder = graphBuilder ?? throw new ArgumentNullException(nameof(graphBuilder));
            _travelPlanProvider = travelPlanProvider ?? throw new ArgumentNullException(nameof(travelPlanProvider));
        }

        public IEnumerable<TravelPlan> GetTravelPlans(Location sourceLocation, Location destinationLocation)
        {
            // TODO: find a way to limit graph within coordinates\
            var coordinateLimits = _coordinateLimitsProvider.GetCoordinateLimits(sourceLocation, destinationLocation);

            var graph = _graphBuilder.GetGraph(coordinateLimits);

            return _travelPlanProvider.GetTravelPlans(graph, sourceLocation, destinationLocation);
        }
    }
}