using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MPK.Connect.DataAccess;
using MPK.Connect.Model;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.DataAccess;
using MPK.Connect.Service.Helpers;

namespace MPK.Connect.Service.Business
{
    public class TimeTableService : ITimeTableService
    {
        private readonly IGenericRepository<StopTime> _stopTimeRepository;
        private readonly IGenericRepository<Stop> _stopRepository;
        private readonly IGenericRepository<Calendar> _calendarRepository;

        public TimeTableService(IGenericRepository<StopTime> stopTimeRepository, IGenericRepository<Stop> stopRepository, IGenericRepository<Calendar> calendarRepository)
        {
            _stopTimeRepository = stopTimeRepository ?? throw new ArgumentNullException(nameof(stopTimeRepository));
            _stopRepository = stopRepository ?? throw new ArgumentNullException(nameof(stopRepository));
            _calendarRepository = calendarRepository ?? throw new ArgumentNullException(nameof(calendarRepository)); ;
        }

        public TimeTable GetTimeTable(string stopId)
        {
            var stop = _stopRepository.FindBy(st => st.Id == stopId).FirstOrDefault();
            if (stop == null)
            {
                return new TimeTable();
            }

            var currentCalendar = GetCurrentCalendar();

            var timeNow = DateTime.Now.TimeOfDay;
            var stopTimes = _stopTimeRepository.GetAll()
                .Include(st => st.Trip)
                .Include(st => st.Trip.Route)
                .Where(st => st.StopId == stopId && timeNow <= st.DepartureTime)
                .Where(st => st.Trip.ServiceId == currentCalendar.ServiceId)
                .Select(st => new StopTimeInfo
                {
                    DepartureTime = st.DepartureTime,
                    TripId = st.TripId,
                    RouteId = st.Trip.Route.Id,
                    RouteType = st.Trip.Route.Type,
                    Direction = st.Trip.HeadSign
                })
                .ToList();

            var groupedStopTimes = stopTimes
                .GroupBy(st => st.RouteId)
                .ToDictionary(k => k.Key,
                    v => new RouteStopTimes
                    {
                        RouteType = v.First().RouteType,
                        Directions = v
                            .GroupBy(sti => sti.Direction)
                            .Select(d => new DirectionStopTimes
                            {
                                Direction = d.Key,
                                StopTimes = d.Select(sti =>
                                    new StopTimeDto
                                    {
                                        DepartureTime = sti.DepartureTime,
                                        TripId = sti.TripId
                                    }).OrderBy(std => std.DepartureTime).ToList()
                            })
                            .OrderBy(d => d.StopTimes.FirstOrDefault()).ToList()
                    });

            return new TimeTable
            {
                StopId = stop.Id,
                StopName = stop.Name,
                StopCode = stop.Code,
                RouteTimes = groupedStopTimes
            };
        }

        private Calendar GetCurrentCalendar()
        {
            var currentDayOfWeek = DateTime.Now.DayOfWeek.ToString();
            return _calendarRepository.FindBy(c => c.GetPropValue<bool>(currentDayOfWeek))
                .FirstOrDefault(); ;
        }
    }
}