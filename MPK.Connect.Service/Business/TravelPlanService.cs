using System;
using System.Collections.Generic;
using System.Linq;
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
            var graph = _graphBuilder.GetGraph();

            // TODO: Select best fit for source and destination location
            var source = graph.Nodes.Values.FirstOrDefault(n =>
                n.Data.Stop.Name.Trim().ToLower() == sourceLocation.Name.Trim().ToLower());

            var destination = graph.Nodes.Values.FirstOrDefault(n =>
                String.Equals(n.Data.Stop.Name.Trim(), destinationLocation.Name.Trim(), StringComparison.CurrentCultureIgnoreCase));

            // TODO: Add new path provider/manager/whatever (maybe paralell for many destinations)
            return _travelPlanProvider.GetTravelPlans(graph, sourceLocation, destinationLocation);
        }
    }
}