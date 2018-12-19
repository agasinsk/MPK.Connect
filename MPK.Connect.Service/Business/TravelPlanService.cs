using System;
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
            var source = graph.Nodes.Values.FirstOrDefault(n =>
                n.Data.Stop.Name.Trim().ToLower() == sourceLocation.Name.Trim().ToLower());

            var destination = graph.Nodes.Values.FirstOrDefault(n =>
                n.Data.Stop.Name.Trim().ToLower() == destinationLocation.Name.Trim().ToLower());

            // TODO: Add new path provider/manager/whatever (maybe paralell for many destinations)
            var path = graph.A_Star(source.Data, destination.Data);

            //
            var now = DateTime.Now;
            var startHour = path.FirstOrDefault().DepartureTime;
            var start = new DateTime(now.Year, now.Month, now.Day, startHour.Hours, startHour.Minutes, startHour.Seconds);
            var endHour = path.FirstOrDefault().DepartureTime;
            var end = new DateTime(now.Year, now.Month, now.Day, endHour.Hours, endHour.Minutes, endHour.Seconds);

            var travelPlan = new TravelPlan
            {
                Source = sourceLocation,
                Destination = destinationLocation,
                Id = new Guid().ToString(),
                StartTime = start,
                EndTime = end,
                Stops = path
            };
            return travelPlan;
        }
    }
}