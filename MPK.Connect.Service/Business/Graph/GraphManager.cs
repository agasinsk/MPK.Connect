using Microsoft.EntityFrameworkCore;
using MPK.Connect.DataAccess;
using MPK.Connect.Model;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Helpers;
using System;
using System.Linq;

namespace MPK.Connect.Service.Business.Graph
{
    public class GraphManager
    {
        private readonly IGenericRepository<Calendar> _calendarRepository;
        private readonly IGenericRepository<Stop> _stopRepository;
        private readonly IGenericRepository<StopTime> _stopTimeRepository;
        private readonly TimeSpan EPSILON = TimeSpan.FromMinutes(1);

        public GraphManager(IGenericRepository<Stop> stopRepository, IGenericRepository<StopTime> stopTimeRepository, IGenericRepository<Calendar> calendarRepository)
        {
            _stopRepository = stopRepository;
            _stopTimeRepository = stopTimeRepository;
            _calendarRepository = calendarRepository;
        }

        public Graph<string, StopTimeInfo> GetGraph(StopMapBounds graphBounds = null)
        {
            var dbStops = _stopRepository.GetAll()
                .Where(s => s.Latitude < graphBounds.MaxLatitude && s.Latitude > graphBounds.MinLatitude && s.Longitude > graphBounds.MaxLongitude && s.Longitude < graphBounds.MinLongitude)
                .Select(s => new StopDto
                {
                    Id = s.Id,
                    Code = s.Code,
                    Longitude = s.Longitude,
                    Latitude = s.Latitude,
                    Name = s.Name
                })
                .AsNoTracking()
                .ToDictionary(s => s.Id);

            var currentCalendar = GetCurrentCalendar();

            var now = DateTime.Now.TimeOfDay;
            var oneHourLater = now + TimeSpan.FromHours(0.5);
            var dbStopTimesQuery = _stopTimeRepository.GetAll()
                .Where(st => now < st.DepartureTime && st.DepartureTime < oneHourLater)
                .Where(st => st.Trip.ServiceId == currentCalendar.ServiceId);

            if (graphBounds != null)
            {
                dbStopTimesQuery = dbStopTimesQuery.Where(st =>
                   dbStops.ContainsKey(st.StopId));
            }

            var dbStopTimes = dbStopTimesQuery
                .Select(st => new StopTimeInfo
                {
                    Id = st.Id,
                    StopId = st.StopId,
                    TripId = st.TripId,
                    Route = st.Trip.Route.ShortName,
                    DepartureTime = st.DepartureTime,
                    ArrivalTime = st.ArrivalTime,
                    StopSequence = st.StopSequence,
                })
                .OrderBy(st => st.DepartureTime)
                .AsNoTracking()
                .ToDictionary(k => k.Id);

            foreach (var stopTimeInfo in dbStopTimes)
            {
                stopTimeInfo.Value.Stop = dbStops[stopTimeInfo.Value.StopId];
            }

            var graph = new Graph<string, StopTimeInfo>(dbStopTimes);

            // Create a directed edge for every bus route segment
            var trips = dbStopTimes.Values.GroupBy(st => st.TripId).ToDictionary(k => k.Key, v => v.OrderBy(st => st.DepartureTime).ThenBy(st => st.StopSequence));
            foreach (var trip in trips)
            {
                var tripTimes = trip.Value.ToList();
                for (var i = 0; i < tripTimes.Count - 1; i++)
                {
                    var source = tripTimes[i];
                    var destination = tripTimes[i + 1];
                    var cost = destination.DepartureTime - source.DepartureTime;

                    graph[source.Id].Neighbors.Add(new GraphEdge<string>(source.Id, destination.Id, cost.Minutes));
                }
            }

            // TODO: Add edges corresponding to "staying on the bus at a stop"

            // TODO: Add edges corresponding to "switching stop of the same name"
            var namedStops = dbStopTimes.Values.GroupBy(st => st.Stop.Name).ToDictionary(k => k.Key, v => v.OrderBy(st => st.DepartureTime));
            foreach (var stop in namedStops)
            {
                var namedStopTimes = stop.Value.ToList();
                foreach (var namedStopTime in namedStopTimes)
                {
                    var stopTimesAfterThis = namedStopTimes.Where(st => st.DepartureTime > namedStopTime.DepartureTime && st.StopId != namedStopTime.StopId).ToList();
                    var source = namedStopTime;
                    foreach (var destination in stopTimesAfterThis)
                    {
                        if (source.DepartureTime < destination.DepartureTime)
                        {
                            var cost = destination.DepartureTime - source.DepartureTime;
                            if (source.TripId != destination.TripId)
                            {
                                cost += EPSILON;
                            }

                            graph[source.Id].Neighbors.Add(new GraphEdge<string>(source.Id, destination.Id, cost.Minutes));
                        }
                    }
                }
            }

            // Add edges corresponding to "switching buses"
            var stopTransfers = dbStopTimes.Values.GroupBy(st => st.StopId).ToDictionary(k => k.Key, v => v.OrderBy(st => st.DepartureTime));
            foreach (var stopTransfer in stopTransfers)
            {
                var stopTransferTimes = stopTransfer.Value.ToList();
                for (var i = 0; i < stopTransferTimes.Count - 1; i++)
                {
                    var source = stopTransferTimes[i];
                    var destination = stopTransferTimes[i + 1];
                    if (source.DepartureTime < destination.DepartureTime)
                    {
                        var cost = destination.DepartureTime - source.DepartureTime;
                        if (source.TripId != destination.TripId)
                        {
                            cost += EPSILON;
                        }

                        graph[source.Id].Neighbors.Add(new GraphEdge<string>(source.Id, destination.Id, cost.Minutes));
                    }
                }
            }

            return graph;
        }

        public Graph<string, Stop> GetStopGraph()
        {
            var dbStops = _stopRepository.GetAll().AsNoTracking().ToDictionary(s => s.Id);

            var graph = new Graph<string, Stop>(dbStops);

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

                    graph[source.StopId].Neighbors.Add(new StopGraphEdge<string>
                    {
                        Cost = cost.Minutes,
                        SourceId = source.StopId,
                        DestinationId = destination.StopId,
                        TripId = trip.Key,
                        DepartureTime = source.DepartureTime,
                        ArrivalTime = destination.ArrivalTime
                    });
                }
            }

            return graph;
        }

        public Graph<string, Stop> GetStopGraph(StopMapBounds graphBounds)
        {
            var dbStops = _stopRepository.GetAll()
                .Where(s => s.Latitude < graphBounds.MaxLatitude && s.Latitude > graphBounds.MinLatitude && s.Longitude > graphBounds.MaxLongitude && s.Longitude < graphBounds.MinLongitude)
                .AsNoTracking()
                .ToDictionary(s => s.Id);

            var graph = new Graph<string, Stop>(dbStops);

            var now = DateTime.Now.TimeOfDay;
            var oneHourLater = now + TimeSpan.FromHours(0.5);
            var dbStopTimes = _stopTimeRepository.GetAll()
                .Where(st => dbStops.Keys.Contains(st.StopId))
                .Where(st => now < st.DepartureTime && st.DepartureTime < oneHourLater)
                .Select(st => new { st.StopId, st.Trip.RouteId, st.TripId, st.ArrivalTime, st.DepartureTime, st.StopSequence })
                .AsNoTracking()
                .ToList();

            var trips = dbStopTimes.GroupBy(st => st.TripId)
                .ToDictionary(k => k.Key, v => v.OrderBy(st => st.DepartureTime).ThenBy(st => st.StopSequence));

            foreach (var trip in trips)
            {
                var tripTimes = trip.Value.ToList();
                for (var i = 0; i < tripTimes.Count - 1; i++)
                {
                    var source = tripTimes[i];
                    var destination = tripTimes[i + 1];
                    var cost = source.DepartureTime - now + (destination.ArrivalTime - source.DepartureTime);

                    graph[source.StopId].Neighbors.Add(new StopGraphEdge<string>
                    {
                        Cost = cost.Minutes,
                        SourceId = source.StopId,
                        DestinationId = destination.StopId,
                        DepartureTime = source.DepartureTime,
                        ArrivalTime = destination.ArrivalTime,
                        RouteId = source.RouteId,
                        TripId = trip.Key,
                    });
                }
            }

            return graph;
        }

        private Calendar GetCurrentCalendar()
        {
            var currentDayOfWeek = DateTime.Now.DayOfWeek.ToString();
            return _calendarRepository.FindBy(c => c.GetPropValue<bool>(currentDayOfWeek))
                .FirstOrDefault(); ;
        }
    }
}