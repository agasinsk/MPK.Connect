using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess;
using MPK.Connect.Model;
using MPK.Connect.Model.Business;

namespace MPK.Connect.Service.Business
{
    public class StopTimeService : IStopTimeService
    {
        private readonly IGenericRepository<StopTime> _stopTimeRepository;
        private readonly ILogger<StopTimeService> _logger;

        public StopTimeService(IGenericRepository<StopTime> stopTimeRepository, ILogger<StopTimeService> logger)
        {
            _stopTimeRepository = stopTimeRepository ?? throw new ArgumentNullException(nameof(stopTimeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public StopTimeDto UpdateStopTime(StopTimeUpdateInfo stopTimeUpdateInfo)
        {
            var stopTime = _stopTimeRepository.FindBy(st =>
                    st.TripId == stopTimeUpdateInfo.TripId
                    && st.StopId == stopTimeUpdateInfo.StopId
                    && st.DepartureTime == stopTimeUpdateInfo.DepartureTime)
                .SingleOrDefault();

            if (stopTime == null)
            {
                _logger.LogError($"Stop time with {stopTimeUpdateInfo} was not found!");
                return null;
            }

            stopTime.ArrivalTime = stopTimeUpdateInfo.UpdatedDepartureTime;
            stopTime.DepartureTime = stopTimeUpdateInfo.UpdatedDepartureTime;

            _stopTimeRepository.Save();

            return new StopTimeDto { TripId = stopTime.TripId, DepartureTime = stopTime.DepartureTime };
        }

        public StopTimeDto DeleteStopTime(StopTimeInfo stopTimeUpdateInfo)
        {
            var stopTime = _stopTimeRepository.FindBy(st =>
                    st.TripId == stopTimeUpdateInfo.TripId
                    && st.StopId == stopTimeUpdateInfo.StopId
                    && st.DepartureTime == stopTimeUpdateInfo.DepartureTime)
                .SingleOrDefault();

            if (stopTime == null)
            {
                _logger.LogError($"Stop time with {stopTimeUpdateInfo} was not found!");
                return null;
            }

            _stopTimeRepository.Delete(stopTime);
            _stopTimeRepository.Save();

            return new StopTimeDto { TripId = stopTime.TripId, DepartureTime = stopTime.DepartureTime };
        }
    }
}