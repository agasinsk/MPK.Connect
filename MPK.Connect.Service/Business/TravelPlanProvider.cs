using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Business.Graph;

namespace MPK.Connect.Service.Business
{
    public class TravelPlanProvider : ITravelPlanProvider
    {
        private readonly IPathFinder _pathFinder;
        private readonly IMapper _mapper;

        public TravelPlanProvider(IPathFinder pathFinder, IMapper mapper)
        {
            _pathFinder = pathFinder ?? throw new ArgumentNullException(nameof(pathFinder));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IEnumerable<TravelPlan> GetTravelPlans(Graph<string, StopTimeInfo> graph, Location sourceLocation, Location destinationLocation)
        {
            // Get source node
            var source = graph.Nodes
                .Where(s => s.Value.Data.Stop.Name.Trim().ToLower() == sourceLocation.Name.Trim().ToLower())
                .OrderBy(s => s.Value.Data.DepartureTime).First().Value;

            // Get all destinations
            var destinations = graph.Nodes
                .Where(s => s.Value.Data.Stop.Name.Trim().ToLower() == destinationLocation.Name.Trim().ToLower()
                            && s.Value.Data.DepartureTime > source.Data.DepartureTime)
                .OrderBy(s => s.Value.Data.DepartureTime)
                .Select(s => s.Value)
                .ToList();

            // Search for shortest path to various destinations
            var paths = new ConcurrentBag<Path<StopTimeInfo>>();
            Parallel.ForEach(destinations, destination =>
            {
                var path = _pathFinder.FindShortestPath(graph, source.Data, destination.Data);
                if (path.Any())
                {
                    paths.Add(path);
                }
            });

            return _mapper.Map<List<TravelPlan>>(paths.OrderBy(p => p.Cost));
        }
    }
}