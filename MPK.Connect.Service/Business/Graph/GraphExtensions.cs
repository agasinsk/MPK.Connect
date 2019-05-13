﻿using MPK.Connect.Model.Business;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Utils;
using System.Collections.Generic;
using System.Linq;

namespace MPK.Connect.Service.Business.Graph
{
    public static class GraphExtensions
    {
        public static StopDto GetReferenceDestinationStop(this Graph<int, StopTimeInfo> graph, string destinationName)
        {
            return graph.Nodes.Values
                .First(s => s.Data.StopDto.Name.TrimToLower() == destinationName.TrimToLower())
                .Data.StopDto;
        }

        public static List<GraphNode<int, StopTimeInfo>> GetSourceNodes(this Graph<int, StopTimeInfo> graph, string sourceName, StopDto referentialDestinationStop)
        {
            // Get source stops that have the same name
            var sourceStopTimesGroupedByStop = graph.Nodes.Values
                .Where(s => s.Data.StopDto.Name.TrimToLower() == sourceName.TrimToLower())
                .GroupBy(s => s.Data.StopDto)
                .ToDictionary(k => k.Key, v => v.GroupBy(st => st.Data.Route)
                    .SelectMany(gr => gr.GroupBy(st => st.Data.DirectionId)
                        .Select(g => g.OrderBy(st => st.Data.DepartureTime)
                            .First().Id))
                    .ToList());

            // Get only stops with neighbors closer to destination
            var stopsWithRightDirectionIds = new List<int>();

            foreach (var stopWithTimes in sourceStopTimesGroupedByStop)
            {
                // Get neighbor stops
                var neighborStops = stopWithTimes.Value
                    .SelectMany(stopTimeInfoId => graph.GetNeighborsQueryable(stopTimeInfoId)
                        .Select(n => n.Data.StopDto)
                        .Where(s => s.Name.TrimToLower() != sourceName.TrimToLower()))
                    .Distinct()
                    .ToList();

                // Calculate straight-line distance from stop to destination
                var distanceToDestination = stopWithTimes.Key.GetDistanceTo(referentialDestinationStop);

                if (neighborStops.Any())
                {
                    var minimumDistanceToNeighbor = neighborStops
                        .Select(s => s.GetDistanceTo(referentialDestinationStop))
                        .Min();

                    // Take only those stops which have neighbors closer to the destination
                    if (minimumDistanceToNeighbor < distanceToDestination)
                    {
                        stopsWithRightDirectionIds.Add(stopWithTimes.Key.Id);
                    }
                }
            }

            // Get matching graph nodes (get only one node per route id and stop id)
            var filteredSourceNodes = graph.Nodes.Values
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