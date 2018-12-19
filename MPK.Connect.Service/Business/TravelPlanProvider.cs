using System;
using System.Collections.Generic;
using System.Linq;
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
            var source = graph.Nodes
                .Where(s => s.Value.Data.Stop.Name.Trim().ToLower() == sourceLocation.Name.Trim().ToLower())
                .OrderBy(s => s.Value.Data.DepartureTime).First().Value;

            var destinations = graph.Nodes
                .Where(s => s.Value.Data.Stop.Name.Trim().ToLower() == destinationLocation.Name.Trim().ToLower()
                            && s.Value.Data.DepartureTime > source.Data.DepartureTime)
                .OrderBy(s => s.Value.Data.DepartureTime)
                .Select(s => s.Value)
                .ToList();

            var paths = new List<Path<StopTimeInfo>>();
            foreach (var destination in destinations)
            {
                var path = graph.A_Star(source.Data, destination.Data);
                paths.Add(path);
            }

            paths = paths.Where(p => p.Any()).OrderBy(p => p.Cost).ToList();
            var mappedTravelPlans = _mapper.Map<List<TravelPlan>>(paths);
            return mappedTravelPlans;
        }
    }
}