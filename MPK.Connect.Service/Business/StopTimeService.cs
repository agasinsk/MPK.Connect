using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess;
using MPK.Connect.Model;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Technical;

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

        public ApiResponse<StopTimeDto> UpdateStopTime(StopTimeUpdateDto stopTimeUpdateDto)
        {
            var stopTime = _stopTimeRepository.FindBy(st =>
                    st.TripId == stopTimeUpdateDto.TripId
                    && st.StopId == stopTimeUpdateDto.StopId
                    && st.DepartureTime == stopTimeUpdateDto.DepartureTime)
                .SingleOrDefault();

            if (stopTime == null)
            {
                _logger.LogError($"Stop time with {stopTimeUpdateDto} was not found!");
                return new ErrorResponse<StopTimeDto>(null);
            }

            stopTime.ArrivalTime = stopTimeUpdateDto.UpdatedDepartureTime;
            stopTime.DepartureTime = stopTimeUpdateDto.UpdatedDepartureTime;

            _stopTimeRepository.Save();

            return new OkResponse<StopTimeDto>(stopTimeUpdateDto);
        }

        public ApiResponse<StopTimeDto> DeleteStopTime(StopTimeDto stopTimeDto)
        {
            var stopTime = _stopTimeRepository.FindBy(st =>
                    st.TripId == stopTimeDto.TripId
                    && st.StopId == stopTimeDto.StopId
                    && st.DepartureTime == stopTimeDto.DepartureTime)
                .SingleOrDefault();

            if (stopTime == null)
            {
                _logger.LogError($"Stop time with {stopTimeDto} was not found!");
                return new ErrorResponse<StopTimeDto>(null, $"Stop time with {stopTimeDto} was not found!");
            }

            _stopTimeRepository.Delete(stopTime);
            _stopTimeRepository.Save();

            return new OkResponse<StopTimeDto>(stopTimeDto);
        }
    }
}