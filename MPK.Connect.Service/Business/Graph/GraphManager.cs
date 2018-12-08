using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MPK.Connect.DataAccess;
using MPK.Connect.Model;

namespace MPK.Connect.Service.Business.Graph
{
    public class GraphManager
    {
        private readonly IGenericRepository<StopTime> _stopTimeRepository;

        public GraphManager(IGenericRepository<StopTime> stopTimeRepository)
        {
            _stopTimeRepository = stopTimeRepository ?? throw new ArgumentNullException(nameof(stopTimeRepository));
        }

        public IEnumerable<string> InitializeStopTimeGraph()
        {
            var now = DateTime.Now.TimeOfDay;
            var oneHourLater = now + TimeSpan.FromHours(1);
            var dbStopTimes = _stopTimeRepository.GetAll()
                .Where(st => now < st.DepartureTime && st.DepartureTime < oneHourLater)
                .Select(st => new { st.StopId, st.TripId, st.DepartureTime, st.StopSequence })
                .AsNoTracking()
                .ToList();

            return new List<string>();
        }
    }
}