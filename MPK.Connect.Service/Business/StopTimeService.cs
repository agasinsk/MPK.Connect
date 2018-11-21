using System;
using System.Linq;
using MPK.Connect.DataAccess;
using MPK.Connect.Model;
using MPK.Connect.Model.Business;

namespace MPK.Connect.Service.Business
{
    public class StopTimeService : IStopTimeService
    {
        private readonly IGenericRepository<StopTime> _stopTimeRepository;

        public StopTimeService(IGenericRepository<StopTime> stopTimeRepository)
        {
            _stopTimeRepository = stopTimeRepository ?? throw new ArgumentNullException(nameof(stopTimeRepository));
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
                return new StopTimeDto();
            }

            _stopTimeRepository.Delete(stopTime);
            _stopTimeRepository.Save();

            return new StopTimeDto { TripId = stopTime.TripId, DepartureTime = stopTime.DepartureTime };
        }
    }
}