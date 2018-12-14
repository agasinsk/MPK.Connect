using MPK.Connect.DataAccess;
using MPK.Connect.Model;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Service.Business.Graph;
using System;
using System.Linq;

namespace MPK.Connect.Service.Business
{
    public class TravelPlanService : ITravelPlanService
    {
        private readonly IGraphBuilder _graphBuilder;
        private readonly IGenericRepository<StopTime> _stopRepository;

        public TravelPlanService(IGraphBuilder graphBuilder, IGenericRepository<StopTime> stopTimeRepository)
        {
            _graphBuilder = graphBuilder ?? throw new ArgumentNullException(nameof(graphBuilder));
            _stopRepository = stopTimeRepository ?? throw new ArgumentNullException(nameof(stopTimeRepository)); ;
        }

        public TravelPlan GetTravelPlan(Location sourceLocation, Location destinationLocation)
        {
            var graph = _graphBuilder.GetGraph();

            // TODO: Select best fit for source and destination location
            var source = graph.Nodes.Values.Where(n =>
                n.Data.Stop.Name.Trim().ToLower() == sourceLocation.Name.Trim().ToLower());

            // TODO: Add new path provider/manager/whatever (maybe paralell for many destinations)

            //

            return new TravelPlan();
        }
    }
}