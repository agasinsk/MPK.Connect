using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MoreLinq.Extensions;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Business.Graph;

namespace MPK.Connect.Service.Business
{
    public class TravelPlanService : ITravelPlanService
    {
        private readonly IGraphBuilder _graphBuilder;
        private readonly IPathProvider _pathProvider;
        private readonly ILogger<TravelPlanService> _logger;
        private readonly IMapper _mapper;

        public TravelPlanService(IGraphBuilder graphBuilder, IPathProvider pathProvider, ILogger<TravelPlanService> logger, IMapper mapper)
        {
            _graphBuilder = graphBuilder ?? throw new ArgumentNullException(nameof(graphBuilder));
            _pathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IEnumerable<TravelPlan> GetTravelPlans(TravelOptions travelOptions)
        {
            var source = travelOptions.Source;
            var destination = travelOptions.Destination;
            ValidateLocations(source, destination);

            var startDate = travelOptions.StartDate ?? DateTime.Now;
            var graph = _graphBuilder.GetGraph(startDate);

            // Get the names of the closest stops by location coordinates
            if (string.IsNullOrEmpty(source.Name) || string.IsNullOrEmpty(destination.Name))
            {
                UpdatedLocationsWithNamesOfTheClosestStops(source, destination, graph);
            }

            // Get available paths from source to destination
            var availablePaths = _pathProvider.GetAvailablePaths(graph, source, destination);
            availablePaths.ForEach(p => p.StartDate = startDate);
            var travelPlans = _mapper.Map<List<TravelPlan>>(availablePaths);

            return travelPlans.OrderBy(t => t.Transfers).ThenBy(t => t.StartTime).ThenBy(t => t.Duration);
        }

        /// <summary>
        /// Maps the available paths to travel plans and sorts them into categories
        /// </summary>
        /// <param name="availablePaths">Paths from source to destination</param>
        /// <returns>Hierarchy of travel plans</returns>
        private Dictionary<TravelPlanCategories, IEnumerable<TravelPlan>> GetTravelPlanHierarchy(List<Path<StopTimeInfo>> availablePaths)
        {
            if (!availablePaths.Any())
            {
                return new Dictionary<TravelPlanCategories, IEnumerable<TravelPlan>>();
            }

            var travelPlans = _mapper.Map<List<TravelPlan>>(availablePaths);

            var minimumTransfersCount = travelPlans.Min(t => t.Transfers);
            var comfortableTravelPlans = travelPlans.Where(t => t.Transfers == minimumTransfersCount).ToList();

            var travelPlanHierarchy = new Dictionary<TravelPlanCategories, IEnumerable<TravelPlan>>
            {
                { TravelPlanCategories.Comfortable, comfortableTravelPlans},
                { TravelPlanCategories.Fast, travelPlans.Except(comfortableTravelPlans)}
            };

            return travelPlanHierarchy;
        }

        private StopDto GetClosestStop(Location source, IEnumerable<StopDto> stops)
        {
            return stops.Aggregate((l, r) =>
                l.GetDistanceTo(source.Latitude.Value, source.Longitude.Value) <
                r.GetDistanceTo(source.Latitude.Value, source.Longitude.Value)
                    ? l : r);
        }

        private void UpdatedLocationsWithNamesOfTheClosestStops(Location source, Location destination, Graph<string, StopTimeInfo> graph)
        {
            var stops = graph.Nodes.Select(st => st.Value.Data.StopDto).DistinctBy(s => s.Id).ToList();
            if (string.IsNullOrEmpty(source.Name))
            {
                var sourceStop = GetClosestStop(source, stops);
                source.Name = sourceStop.Name;
            }
            if (string.IsNullOrEmpty(destination.Name))
            {
                var destinationStop = GetClosestStop(destination, stops);
                destination.Name = destinationStop.Name;
            }
        }

        private void ValidateLocations(params Location[] locations)
        {
            foreach (var location in locations)
            {
                if (ValidateLocation(location))
                {
                    var locationException = new ArgumentException($"The {nameof(location)} was incorrect. It should either have a name or coordinates specified.", nameof(location));

                    _logger.LogError(locationException, $"The {nameof(location)} was incorrect.");
                    throw locationException;
                }
            }
        }

        private bool ValidateLocation(Location location)
        {
            return string.IsNullOrEmpty(location.Name) && (!location.Latitude.HasValue || !location.Longitude.HasValue);
        }
    }
}