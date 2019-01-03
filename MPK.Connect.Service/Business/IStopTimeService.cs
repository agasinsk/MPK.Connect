using MPK.Connect.Model.Business;
using MPK.Connect.Model.Technical;

namespace MPK.Connect.Service.Business
{
    public interface IStopTimeService
    {
        ApiResponse<StopTimeDto> DeleteStopTime(int stopTimeId);

        ApiResponse<StopTimeDto> UpdateStopTime(StopTimeUpdateDto stopTimeUpdateDto);
    }
}