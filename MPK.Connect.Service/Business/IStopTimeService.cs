using MPK.Connect.Model.Business;

namespace MPK.Connect.Service.Business
{
    public interface IStopTimeService
    {
        StopTimeDto DeleteStopTime(StopTimeInfo stopTimeUpdateInfo);

        StopTimeDto UpdateStopTime(StopTimeUpdateInfo stopTimeUpdateInfo);
    }
}