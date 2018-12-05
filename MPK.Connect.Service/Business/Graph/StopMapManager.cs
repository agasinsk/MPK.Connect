using System;
using System.Collections.Generic;
using System.Linq;
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

        public void InitializeGraph()
        {
            var dbStops = _stopRepository.GetAll().ToDictionary(s => s.Id, s => new StopGraphNode(s));

            var primitiveGraph = new Dictionary<string, StopGraphNode>(dbStops);

            var now = DateTime.Now.TimeOfDay;
            var oneHourLater = now + TimeSpan.FromHours(1);
            var dbStopTimes = _stopTimeRepository.GetAll()
                .Where(st => now < st.DepartureTime && st.DepartureTime < oneHourLater)
                .Select(st => new { st.StopId, st.TripId, st.DepartureTime, st.StopSequence })
                .ToList();

            var trips = dbStopTimes.GroupBy(st => st.TripId).ToDictionary(k => k.Key, v => v.OrderBy(st => st.StopSequence));

            foreach (var trip in trips)
            {
                var tripTimes = trip.Value.ToList();
                for (var i = 0; i < tripTimes.Count - 1; i++)
                {
                    var source = tripTimes[i];
                    var destination = tripTimes[i + 1];
                    var cost = source.DepartureTime - now + (destination.DepartureTime - source.DepartureTime);

                    primitiveGraph[source.StopId].Neighbors.Add(new StopGraphEdge { Cost = cost, StopId = destination.StopId, TripId = trip.Key });
                }
            }
        }
    }
}