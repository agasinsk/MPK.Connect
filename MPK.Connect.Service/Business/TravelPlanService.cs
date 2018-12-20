using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using MoreLinq.Extensions;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Service.Business.Graph;

namespace MPK.Connect.Service.Business
{
    public class TravelPlanService : ITravelPlanService
    {
        private readonly IGraphBuilder _graphBuilder;
        private readonly ITravelPlanProvider _travelPlanProvider;
        private readonly ILogger<TravelPlanService> _logger;

        public TravelPlanService(IGraphBuilder graphBuilder, ITravelPlanProvider travelPlanProvider, ILogger<TravelPlanService> logger)
        {
            _graphBuilder = graphBuilder ?? throw new ArgumentNullException(nameof(graphBuilder));
            _travelPlanProvider = travelPlanProvider ?? throw new ArgumentNullException(nameof(travelPlanProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<TravelPlan> GetTravelPlans(Location source, Location destination)
        {
            ValidateLocations(source, destination);

            var graph = _graphBuilder.GetGraph();

            if (string.IsNullOrEmpty(source.Name))
            {
                var stops = graph.Nodes.Select(st => st.Value.Data.Stop).DistinctBy(s => s.Id).ToList();
                var sourceStop = stops.Aggregate((l, r) => l.GetDistanceTo(source.Latitude.Value, source.Longitude.Value) < r.GetDistanceTo(source.Latitude.Value, source.Longitude.Value) ? l : r);
                source.Name = sourceStop.Name;
            }

            if (string.IsNullOrEmpty(destination.Name))
            {
                var stops = graph.Nodes.Select(st => st.Value.Data.Stop).DistinctBy(s => s.Id).ToList();
                var destinationStop = stops.Aggregate((l, r) => l.GetDistanceTo(destination.Latitude.Value, destination.Longitude.Value) < r.GetDistanceTo(destination.Latitude.Value, destination.Longitude.Value) ? l : r);
                destination.Name = destinationStop.Name;
            }

            return _travelPlanProvider.GetTravelPlans(graph, source, destination);
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