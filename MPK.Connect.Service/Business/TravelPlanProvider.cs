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
using MPK.Connect.Service.Helpers;

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

        /// <summary>
        /// Gets travel plans from source to destination
        /// </summary>
        /// <param name="graph">Graph with stop times</param>
        /// <param name="sourceLocation">Source</param>
        /// <param name="destinationLocation">Destination</param>
        /// <returns>Collection of probable paths from source to destination</returns>
        public IEnumerable<TravelPlan> GetTravelPlans(Graph<string, StopTimeInfo> graph, Location sourceLocation, Location destinationLocation)
        {
            // TODO: experiment with different approaches to selecting starting nodes
            // Get source nodes
            var sources = graph.Nodes.Values
                .Where(s => s.Data.StopDto.Name.TrimToLower() == sourceLocation.Name.TrimToLower())
                .GroupBy(s => s.Data.StopId)
                .SelectMany(g => g.OrderBy(st => st.Data.DepartureTime).Take(2))
                .ToList();

            // Search for shortest path from subsequent sources to destination
            var paths = new ConcurrentBag<Path<StopTimeInfo>>();
            Parallel.ForEach(sources, source =>
            {
                var path = _pathFinder.FindShortestPath(graph, source.Data, destinationLocation.Name);
                if (path.Any())
                {
                    paths.Add(path);
                }
            });

            var filteredPaths = paths.OrderBy(p => p.First().DepartureTime)
                .ThenBy(p => p.Cost)
                .ToList();

            return _mapper.Map<List<TravelPlan>>(filteredPaths);
        }
    }
}