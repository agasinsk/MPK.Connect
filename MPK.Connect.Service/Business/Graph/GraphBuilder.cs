using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MPK.Connect.DataAccess;
using MPK.Connect.Model;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Helpers;

namespace MPK.Connect.Service.Business.Graph
{
    public class GraphBuilder : IGraphBuilder
    {
        private readonly TimeSpan _additionalStopChangeTime = TimeSpan.FromMinutes(1);
        private readonly TimeSpan _additionalTransferTime = TimeSpan.FromMinutes(1);
        private readonly IGenericRepository<Calendar> _calendarRepository;
        private readonly TimeSpan _maxStopTimeDepartureTime = TimeSpan.FromHours(0.5);
        private readonly TimeSpan _minimumSwitchingTime = TimeSpan.FromMinutes(1);
        private readonly IGenericRepository<Stop> _stopRepository;
        private readonly IGenericRepository<StopTime> _stopTimeRepository;

        public GraphBuilder(IGenericRepository<Stop> stopRepository, IGenericRepository<StopTime> stopTimeRepository, IGenericRepository<Calendar> calendarRepository)
        {
            _stopRepository = stopRepository ?? throw new ArgumentNullException(nameof(stopRepository));
            _stopTimeRepository = stopTimeRepository ?? throw new ArgumentNullException(nameof(stopTimeRepository));
            _calendarRepository = calendarRepository ?? throw new ArgumentNullException(nameof(calendarRepository));
        }

        /// <summary>
        /// Builds graph based on specified geographical limits and time bounds
        /// Nodes are StopTime entities, the edges are connections between StopTimes
        /// </summary>
        /// <param name="graphLimits">Geographical limits for stop locations</param>
        /// <returns>Graph of stop times</returns>
        public Graph<string, StopTimeInfo> GetGraph(CoordinateLimits graphLimits = null)
        {
            // Get stops matching the bounds
            var dbStops = GetStops(graphLimits);

            // Get calendar matching current day of the week
            var currentCalendar = GetCurrentCalendar();

            // Get stop times for required stops
            var dbStopTimes = GetStopTimes(dbStops, currentCalendar.ServiceId);

            var graph = new Graph<string, StopTimeInfo>(dbStopTimes);

            // Create a directed edge for every bus route segment
            CreateDirectedEdgesWithinEachTrip(dbStopTimes, graph);

            // TODO: Add edges corresponding to "staying on the bus at a stop" (if any)

            // Add edges corresponding to switching stops of the same name
            CreateDirectedEdgesForSwitchingStopsWithSameName(dbStopTimes, graph);

            // Add edges corresponding to switching buses (trips)
            CreateDirectedEdgesForSwitchingTrips(dbStopTimes, graph);

            return graph;
        }

        /// <summary>
        /// Groups stop times by stop name and creates edges between the different stops of the same name
        /// </summary>
        /// <param name="dbStopTimes">Stop times</param>
        /// <param name="graph">Graph</param>
        private void CreateDirectedEdgesForSwitchingStopsWithSameName(Dictionary<string, StopTimeInfo> dbStopTimes, Graph<string, StopTimeInfo> graph)
        {
            var stopTimesGroupedByStopName = dbStopTimes.Values.GroupBy(st => st.Stop.Name).ToDictionary(k => k.Key, v => v.AsEnumerable());
            foreach (var stopTimesGroup in stopTimesGroupedByStopName)
            {
                var stopTimesWithTheSameStopName = stopTimesGroup.Value.ToList();
                foreach (var sourceStopTime in stopTimesWithTheSameStopName)
                {
                    var stopTimesAfterSource = stopTimesWithTheSameStopName.Where(st => sourceStopTime.DepartureTime < st.DepartureTime && st.StopId != sourceStopTime.StopId);
                    foreach (var destination in stopTimesAfterSource)
                    {
                        var cost = destination.DepartureTime - sourceStopTime.DepartureTime;

                        // TODO: Consider adding distance as a factor for additional cost
                        //var distance = source.GetDistanceTo(destination);
                        //if (distance > 0.2 && cost.Minutes < 2)
                        //{
                        //    cost += _additionalStopChangeTime;
                        //}

                        graph[sourceStopTime.Id].Neighbors.Add(new GraphEdge<string>(sourceStopTime.Id, destination.Id, cost.Minutes));
                    }
                }
            }
        }

        /// <summary>
        /// Groups stop times by stop id and creates edges corrensponding to switching the trips (routes)
        /// </summary>
        /// <param name="dbStopTimes">Stop times</param>
        /// <param name="graph">Graph</param>
        private void CreateDirectedEdgesForSwitchingTrips(Dictionary<string, StopTimeInfo> dbStopTimes, Graph<string, StopTimeInfo> graph)
        {
            var stopTimesGroupedByStopId = dbStopTimes.Values.GroupBy(st => st.StopId).ToDictionary(k => k.Key, v => v.OrderBy(st => st.DepartureTime));
            foreach (var stopTransfers in stopTimesGroupedByStopId)
            {
                var stopTransferTimes = stopTransfers.Value.ToList();
                for (var i = 0; i < stopTransferTimes.Count - 1; i++)
                {
                    var source = stopTransferTimes[i];
                    var destination = stopTransferTimes[i + 1];
                    if (source.DepartureTime + _minimumSwitchingTime < destination.DepartureTime)
                    {
                        var cost = destination.DepartureTime - source.DepartureTime;
                        if (source.TripId != destination.TripId)
                        {
                            cost += _additionalTransferTime;
                        }

                        graph[source.Id].Neighbors.Add(new GraphEdge<string>(source.Id, destination.Id, cost.Minutes));
                    }
                }
            }
        }

        /// <summary>
        /// Groups stop times by trip and creates directed edge between each pair of stop times that belong to the same trip
        /// </summary>
        /// <param name="dbStopTimes">Stop times</param>
        /// <param name="graph">Graph</param>
        private void CreateDirectedEdgesWithinEachTrip(Dictionary<string, StopTimeInfo> dbStopTimes, Graph<string, StopTimeInfo> graph)
        {
            var stopTimesGroupedByTrips = dbStopTimes.Values.GroupBy(st => st.TripId).ToDictionary(k => k.Key, v => v.OrderBy(st => st.StopSequence));
            foreach (var tripStopTimes in stopTimesGroupedByTrips)
            {
                var tripTimes = tripStopTimes.Value.ToList();
                for (var i = 0; i < tripTimes.Count - 1; i++)
                {
                    var source = tripTimes[i];
                    var destination = tripTimes[i + 1];
                    var cost = destination.DepartureTime - source.DepartureTime;

                    graph[source.Id].Neighbors.Add(new GraphEdge<string>(source.Id, destination.Id, cost.Minutes));
                }
            }
        }

        /// <summary>
        /// Gets calendar entity by matching the current day of week
        /// </summary>
        /// <returns>Current calendar</returns>
        private Calendar GetCurrentCalendar()
        {
            var currentDayOfWeek = DateTime.Now.DayOfWeek.ToString();
            return _calendarRepository.FindBy(c => c.GetPropValue<bool>(currentDayOfWeek))
                .FirstOrDefault();
        }

        /// <summary>
        /// Get stops within the provided bounds
        /// </summary>
        /// <param name="coordinateLimits">The geographical bounds</param>
        /// <returns>Stops within bounds</returns>
        private Dictionary<string, StopDto> GetStops(CoordinateLimits coordinateLimits = null)
        {
            var dbStopsQuery = _stopRepository.GetAll();

            if (coordinateLimits != null)
            {
                dbStopsQuery = dbStopsQuery
                    .Where(s => s.Latitude < coordinateLimits.UpperLeftLatitude &&
                                s.Latitude > coordinateLimits.LowerRightLatitude &&
                                s.Longitude > coordinateLimits.UpperLeftLongitude &&
                                s.Longitude < coordinateLimits.LowerRightLongitude);
            }

            return dbStopsQuery
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
        }

        /// <summary>
        /// Gets stop times for specified stops and service id
        /// </summary>
        /// <param name="dbStops">Collection of stops</param>
        /// <param name="serviceId">Id of service (dependent on the day of the week)</param>
        /// <returns>Collection of matching stop times</returns>
        private Dictionary<string, StopTimeInfo> GetStopTimes(Dictionary<string, StopDto> dbStops, string serviceId)
        {
            var now = DateTime.Now.TimeOfDay;
            var later = now + _maxStopTimeDepartureTime;

            var dbStopTimes = _stopTimeRepository.GetAll()
                .Where(st => now < st.DepartureTime && st.DepartureTime < later)
                .Where(st => st.Trip.ServiceId == serviceId)
                .Where(st => dbStops.ContainsKey(st.StopId))
                .Select(st => new StopTimeInfo
                {
                    Id = st.Id,
                    StopId = st.StopId,
                    TripId = st.TripId,
                    Route = st.Trip.Route.ShortName,
                    DepartureTime = st.DepartureTime,
                    ArrivalTime = st.ArrivalTime,
                    StopSequence = st.StopSequence
                })
                .AsNoTracking()
                .ToDictionary(k => k.Id);

            // Assign valid stop to stopTime
            foreach (var stopTimeInfo in dbStopTimes)
            {
                stopTimeInfo.Value.Stop = dbStops[stopTimeInfo.Value.StopId];
            }

            return dbStopTimes;
        }
    }
}