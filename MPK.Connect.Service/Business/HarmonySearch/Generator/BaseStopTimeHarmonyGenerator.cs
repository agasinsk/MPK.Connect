using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Utils;

namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    /// <summary>
    /// Base stop time harmony generator
    /// </summary>
    public abstract class BaseStopTimeHarmonyGenerator : BaseHarmonyGenerator<StopTimeInfo>
    {
        protected readonly Graph<int, StopTimeInfo> Graph;
        protected readonly StopDto ReferentialDestinationStop;
        protected readonly List<GraphNode<int, StopTimeInfo>> SourceNodes;

        public Location Destination { get; }

        public Location Source { get; }

        protected BaseStopTimeHarmonyGenerator(IObjectiveFunction<StopTimeInfo> function, Graph<int, StopTimeInfo> graph, Location destination, Location source) : base(function)
        {
            Graph = graph;
            Destination = destination;
            Source = source;

            // Set up source and destination nodes
            ReferentialDestinationStop = GetReferenceDestinationStop();
            SourceNodes = GetSourceNodes();
        }

        public override Harmony<StopTimeInfo> GenerateRandomHarmony()
        {
            var randomArguments = GetRandomArguments();

            var objectiveValue = ObjectiveFunction.GetObjectiveValue(randomArguments);

            return new Harmony<StopTimeInfo>(objectiveValue, randomArguments);
        }

        protected abstract StopTimeInfo[] GetRandomArguments();

        protected StopDto GetReferenceDestinationStop()
        {
            return Graph.Nodes.Values
                .First(s => s.Data.StopDto.Name.TrimToLower() == Destination.Name.TrimToLower())
                .Data.StopDto;
        }

        protected List<GraphNode<int, StopTimeInfo>> GetSourceNodes()
        {
            // Get source stops that have the same name
            var sourceStopTimesGroupedByStop = Graph.Nodes.Values
                .Where(s => s.Data.StopDto.Name.TrimToLower() == Source.Name.TrimToLower())
                .GroupBy(s => s.Data.StopDto)
                .ToDictionary(k => k.Key, v => v.GroupBy(st => st.Data.Route)
                    .SelectMany(gr => gr.GroupBy(st => st.Data.DirectionId)
                        .Select(g => g.OrderBy(st => st.Data.DepartureTime)
                            .First().Id))
                    .ToList());

            // Calculate straight-line distance to destination
            var distanceToDestination = sourceStopTimesGroupedByStop.Keys
                .Select(s => s.GetDistanceTo(ReferentialDestinationStop)).Max();

            var stopsWithRightDirectionIds = new List<int>();
            foreach (var stopWithTimes in sourceStopTimesGroupedByStop)
            {
                // Get neighbor stops
                var neighborStops = stopWithTimes.Value
                    .SelectMany(stopTimeInfoId => Graph.GetNeighborsQueryable(stopTimeInfoId)
                        .Select(n => n.Data.StopDto)
                        .Where(s => s.Name.TrimToLower() != Source.Name.TrimToLower()))
                    .Distinct()
                    .ToList();

                if (neighborStops.Any())
                {
                    var minimumDistanceToNeighbor = neighborStops
                        .Select(s => s.GetDistanceTo(ReferentialDestinationStop))
                        .Min();

                    // Take only those stops which have neighbors closer to the destination
                    if (minimumDistanceToNeighbor < distanceToDestination)
                    {
                        stopsWithRightDirectionIds.Add(stopWithTimes.Key.Id);
                    }
                }
            }

            // Get matching graph nodes (get only one node per route id and stop id)
            var filteredSourceNodes = Graph.Nodes.Values
                .Where(s => stopsWithRightDirectionIds.Contains(s.Data.StopId))
                .GroupBy(s => s.Data.StopId)
                .SelectMany(g => g.GroupBy(st => st.Data.Route)
                    .SelectMany(gr => gr.GroupBy(st => st.Data.DirectionId)
                        .Select(dg => dg.OrderBy(st => st.Data.DepartureTime).First())))
                .ToList();

            return filteredSourceNodes;
        }
    }
}