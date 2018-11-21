using MPK.Connect.Model.Business;
using MPK.Connect.Model.Technical;

namespace MPK.Connect.Service.Business
{
    public interface IStopTimeService
    {
        ApiResponse<StopTimeDto> DeleteStopTime(StopTimeDto stopTimeDto);

        ApiResponse<StopTimeDto> UpdateStopTime(StopTimeUpdateDto stopTimeUpdateDto);
    }
}