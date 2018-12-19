using System;
using System.Collections.Generic;
using MPK.Connect.DataAccess;
using MPK.Connect.Model;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Service.Business.Graph;

namespace MPK.Connect.Service.Business
{
    public class TravelPlanService : ITravelPlanService
    {
        private readonly IGraphBuilder _graphBuilder;
        private readonly ITravelPlanProvider _travelPlanProvider;
        private readonly IGenericRepository<StopTime> _stopRepository;

        public TravelPlanService(IGraphBuilder graphBuilder, IGenericRepository<StopTime> stopTimeRepository, ITravelPlanProvider travelPlanProvider)
        {
            _graphBuilder = graphBuilder ?? throw new ArgumentNullException(nameof(graphBuilder));
            _stopRepository = stopTimeRepository ?? throw new ArgumentNullException(nameof(stopTimeRepository));
            _travelPlanProvider = travelPlanProvider ?? throw new ArgumentNullException(nameof(travelPlanProvider));
        }

        public IEnumerable<TravelPlan> GetTravelPlans(Location sourceLocation, Location destinationLocation)
        {
            // TODO: find a way to limit graph within coordinates
            var graph = _graphBuilder.GetGraph();

            return _travelPlanProvider.GetTravelPlans(graph, sourceLocation, destinationLocation);
        }
    }
}