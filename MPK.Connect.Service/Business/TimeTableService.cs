using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MPK.Connect.DataAccess;
using MPK.Connect.Model;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.DataAccess;

namespace MPK.Connect.Service.Business
{
    public class TimeTableService : ITimeTableService
    {
        private readonly IGenericRepository<StopTime> _stopTimeRepository;
        private readonly IGenericRepository<Stop> _stopRepository;

        public TimeTableService(IGenericRepository<StopTime> stopTimeRepository, IGenericRepository<Stop> stopRepository)
        {
            _stopTimeRepository = stopTimeRepository ?? throw new ArgumentNullException(nameof(stopTimeRepository));
            _stopRepository = stopRepository ?? throw new ArgumentNullException(nameof(stopRepository));
        }

        public TimeTable GetTimeTable(string stopId)
        {
            var stop = _stopRepository.FindBy(st => st.Id == stopId).FirstOrDefault();
            if (stop == null)
            {
                return new TimeTable();
            }

            var stopTimes = _stopTimeRepository.GetAll()
                .Include(st => st.Trip.Route)
                .Where(st => st.StopId == stopId)
                .Select(st => new StopTimeInfo
                {
                    StopTime = new StopTimeDto
                    {
                        ArrivalTime = st.ArrivalTime,
                        DepartureTime = st.DepartureTime,
                        StopSequence = st.StopSequence
                    },
                    RouteId = st.Trip.Route.Id,
                    RouteType = st.Trip.Route.Type,
                })
                .ToList();

            var groupedStopTimes = stopTimes
                .GroupBy(st => st.RouteId)
                .ToDictionary(k => k.Key, v => new RouteStopTimes
                {
                    RouteType = v.First().RouteType,
                    StopTimes = v.Select(sti => sti.StopTime).ToList()
                });

            return new TimeTable
            {
                StopId = stop.Id,
                StopName = stop.Name,
                StopCode = stop.Code,
                RouteTimes = groupedStopTimes
            };
        }
    }
}