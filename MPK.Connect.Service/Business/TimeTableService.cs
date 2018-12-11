using Microsoft.EntityFrameworkCore;
using MPK.Connect.DataAccess;
using MPK.Connect.Model;
using MPK.Connect.Model.Business;
using MPK.Connect.Service.Helpers;
using System;
using System.Linq;

namespace MPK.Connect.Service.Business
{
    public class TimeTableService : ITimeTableService
    {
        private readonly IGenericRepository<Calendar> _calendarRepository;
        private readonly IGenericRepository<Stop> _stopRepository;
        private readonly IGenericRepository<StopTime> _stopTimeRepository;

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
            var twoHoursLater = timeNow + TimeSpan.FromHours(2);
            var stopTimes = _stopTimeRepository.GetAll()
                .Where(st => st.StopId == stopId && timeNow <= st.DepartureTime && twoHoursLater >= st.DepartureTime)
                .Where(st => st.Trip.ServiceId == currentCalendar.ServiceId)
                .Select(st => new
                {
                    st.DepartureTime,
                    st.TripId,
                    RouteId = st.Trip.Route.Id,
                    RouteType = st.Trip.Route.Type,
                    Direction = st.Trip.HeadSign
                })
                .AsNoTracking()
                .ToList();

            var groupedStopTimes = stopTimes
                .GroupBy(st => st.RouteId)
                .Select(v => new RouteStopTimes
                {
                    RouteId = v.Key,
                    RouteType = v.First().RouteType,
                    Directions = v.GroupBy(sti => sti.Direction)
                        .Select(d => new DirectionStopTimes
                        {
                            Direction = d.Key,
                            StopTimes = d.Select(sti =>
                                new StopTimeCore
                                {
                                    DepartureTime = sti.DepartureTime,
                                    TripId = sti.TripId
                                }).OrderBy(std => std.DepartureTime).ToList()
                        })
                        .ToList()
                })
                .ToList();

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