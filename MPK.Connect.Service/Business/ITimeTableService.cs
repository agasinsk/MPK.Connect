using MPK.Connect.Model.Business;

namespace MPK.Connect.Service.Business
{
    public interface ITimeTableService
    {
        TimeTable GetTimeTable(int stopId);
    }
}