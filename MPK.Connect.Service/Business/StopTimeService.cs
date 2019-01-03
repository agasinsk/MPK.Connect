using AutoMapper;
using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess;
using MPK.Connect.Model;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Technical;
using System;
using System.Linq;

namespace MPK.Connect.Service.Business
{
    public class StopTimeService : IStopTimeService
    {
        private readonly IGenericRepository<StopTime> _stopTimeRepository;
        private readonly ILogger<StopTimeService> _logger;
        private readonly IMapper _mapper;

        public StopTimeService(IGenericRepository<StopTime> stopTimeRepository, ILogger<StopTimeService> logger, IMapper mapper)
        {
            _stopTimeRepository = stopTimeRepository ?? throw new ArgumentNullException(nameof(stopTimeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public ApiResponse<StopTimeDto> UpdateStopTime(StopTimeUpdateDto stopTimeUpdateDto)
        {
            var stopTime = _stopTimeRepository.FindBy(st =>
                    st.Id == stopTimeUpdateDto.Id
                    && st.DepartureTime == stopTimeUpdateDto.DepartureTime)
                .SingleOrDefault();

            if (stopTime == null)
            {
                _logger.LogError($"Stop time with id {stopTimeUpdateDto.Id} was not found!");
                return new ErrorResponse<StopTimeDto>(null, "Wystąpił błąd!");
            }

            stopTime.ArrivalTime = stopTimeUpdateDto.UpdatedDepartureTime;
            stopTime.DepartureTime = stopTimeUpdateDto.UpdatedDepartureTime;

            _stopTimeRepository.Save();

            return new OkResponse<StopTimeDto>(_mapper.Map<StopTimeDto>(stopTime), "Pomyślnie zaktualizowano czas odjazdu!");
        }

        public ApiResponse<StopTimeDto> DeleteStopTime(int stopTimeId)
        {
            var stopTime = _stopTimeRepository.FindBy(st =>
                    st.Id == stopTimeId)
                .SingleOrDefault();

            if (stopTime == null)
            {
                _logger.LogError($"Stop time with id {stopTimeId} was not found!");
                return new ErrorResponse<StopTimeDto>(null, "Wystąpił błąd!");
            }

            _stopTimeRepository.Delete(stopTime);
            _stopTimeRepository.Save();

            return new OkResponse<StopTimeDto>(_mapper.Map<StopTimeDto>(stopTime), "Pomyślnie usunięto!");
        }
    }
}