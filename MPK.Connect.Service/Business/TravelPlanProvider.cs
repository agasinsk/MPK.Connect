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
        private readonly IStopPathFinder _pathFinder;
        private readonly IMapper _mapper;

        public TravelPlanProvider(IStopPathFinder pathFinder, IMapper mapper)
        {
            _pathFinder = pathFinder ?? throw new ArgumentNullException(nameof(pathFinder));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IEnumerable<TravelPlan> GetTravelPlans(Graph<string, StopTimeInfo> graph, Location sourceLocation, Location destinationLocation)
        {
            // Get source nodes
            var sources = graph.Nodes.Values
                .Where(s => s.Data.Stop.Name.Trim().ToLower() == sourceLocation.Name.Trim().ToLower())
                .GroupBy(s => s.Data.Route)
                .Select(g => g.OrderBy(st => st.Data.DepartureTime).First())
                .ToList();

            // Search for shortest path to subsequent destinations
            var paths = new ConcurrentBag<Path<StopTimeInfo>>();
            Parallel.ForEach(sources, source =>
            {
                var path = _pathFinder.FindShortestPath(graph, source.Data, destinationLocation.Name);
                if (path.Any())
                {
                    paths.Add(path);
                }
            });

            return _mapper.Map<List<TravelPlan>>(paths.OrderBy(p => p.Cost));
        }
    }
}