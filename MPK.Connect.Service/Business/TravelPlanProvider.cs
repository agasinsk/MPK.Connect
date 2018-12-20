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
            var sources = GetSourceNodes(graph, sourceLocation, destinationLocation);

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

            // TODO: eliminate duplicated paths
            var filteredPaths = paths.OrderBy(p => p.First().DepartureTime)
                .ThenBy(p => p.Cost)
                .ToList();

            return _mapper.Map<List<TravelPlan>>(filteredPaths);
        }

        private IEnumerable<GraphNode<string, StopTimeInfo>> GetSourceNodes(Graph<string, StopTimeInfo> graph, Location sourceLocation, Location destinationLocation)
        {
            // Get source stops that have the same name
            var sourceStopTimesGroupedByStop = graph.Nodes.Values
                .Where(s => s.Data.StopDto.Name.TrimToLower() == sourceLocation.Name.TrimToLower())
                .GroupBy(s => s.Data.StopDto)
                .ToDictionary(k => k.Key, g => g.Select(s => s.Id).ToList());

            // Get reference to destination location stop
            var referentialDestinationStop = graph.Nodes.Values.First(s => s.Data.StopDto.Name.TrimToLower() == destinationLocation.Name.TrimToLower()).Data.StopDto;

            // Calculate straight-line distance to destination
            var distanceFromStopToDestination = sourceStopTimesGroupedByStop
                .Select(s => s.Key.GetDistanceTo(referentialDestinationStop)).Max();

            var distancesToStops = new Dictionary<string, double>();
            foreach (var stop in sourceStopTimesGroupedByStop)
            {
                // Get neighbor stops
                var neighborStops = stop.Value
                    .SelectMany(st => graph.GetNeighbors(st)
                        .Select(n => n.Data.StopDto)
                        .Where(n => n.Name.TrimToLower() != sourceLocation.Name.TrimToLower()))
                    .Distinct()
                    .ToList();

                if (neighborStops.Any())
                {
                    var minimumDistanceToNeighbor =
                        neighborStops.Select(s => s.GetDistanceTo(referentialDestinationStop)).Min();
                    distancesToStops[stop.Key.Id] = minimumDistanceToNeighbor;
                }
            }

            // Take only those stops which have neighbors closer to the destination
            var stopsWithRightDirectionIds = distancesToStops.Where(s => s.Value < distanceFromStopToDestination).OrderBy(s => s.Value).Select(k => k.Key).ToList();

            // Get matching graph nodes
            var filteredSourceNodes = graph.Nodes.Values
                .Where(s => stopsWithRightDirectionIds.Contains(s.Data.StopId))
                .GroupBy(s => s.Data.StopId)
                .Select(gr => gr.OrderBy(st => st.Data.DepartureTime).First())
                //.SelectMany(g => g.GroupBy(st => st.Data.Route).Select(gr => gr.OrderBy(st => st.Data.DepartureTime).First()))
                .ToList();

            return filteredSourceNodes;
        }
    }
}