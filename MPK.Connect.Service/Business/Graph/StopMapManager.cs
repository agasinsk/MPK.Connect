using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MPK.Connect.DataAccess;
using MPK.Connect.Model;
using MPK.Connect.Model.Graph;

namespace MPK.Connect.Service.Business.Graph
{
    public class StopMapManager : IStopMapManager
    {
        private readonly IGenericRepository<Stop> _stopRepository;
        private readonly IGenericRepository<StopTime> _stopTimeRepository;

        public StopMapManager(IGenericRepository<Stop> stopRepository, IGenericRepository<StopTime> stopTimeRepository)
        {
            _stopRepository = stopRepository;
            _stopTimeRepository = stopTimeRepository;
        }

        public IEnumerable<string> InitializeGraph()
        {
            var primitiveGraph = GetGraph();

            var path = primitiveGraph.FindShortestPath("1418", "2033");

            return path.Select(n => $"{n.SourceStopId} -> {n.DestinationStopId} : trip {n.TripId} : time {n.ArrivalTime}").ToList();
        }

        private Dictionary<string, StopGraphNode> GetGraph()
        {
            var dbStops = _stopRepository.GetAll().AsNoTracking().ToDictionary(s => s.Id, s => new StopGraphNode(s));
            var primitiveGraph = new Dictionary<string, StopGraphNode>(dbStops);

            var now = DateTime.Now.TimeOfDay;
            var oneHourLater = now + TimeSpan.FromHours(1);
            var dbStopTimes = _stopTimeRepository.GetAll()
                .Where(st => now < st.DepartureTime && st.DepartureTime < oneHourLater)
                .Select(st => new { st.StopId, st.TripId, st.ArrivalTime, st.DepartureTime, st.StopSequence })
                .AsNoTracking()
                .ToList();

            var trips = dbStopTimes.GroupBy(st => st.TripId).ToDictionary(k => k.Key, v => v.OrderBy(st => st.DepartureTime).ThenBy(st => st.StopSequence));

            foreach (var trip in trips)
            {
                var tripTimes = trip.Value.ToList();
                for (var i = 0; i < tripTimes.Count - 1; i++)
                {
                    var source = tripTimes[i];
                    var destination = tripTimes[i + 1];
                    var cost = source.DepartureTime - now + (destination.DepartureTime - source.DepartureTime);

                    primitiveGraph[source.StopId].Neighbors.Add(new StopGraphEdge
                    {
                        Cost = cost.Minutes,
                        ArrivalTime = destination.ArrivalTime,
                        DepartureTime = source.DepartureTime,
                        SourceStopId = source.StopId,
                        DestinationStopId = destination.StopId,
                        TripId = trip.Key
                    });
                }
            }

            return primitiveGraph;
        }
    }
}