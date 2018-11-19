using MPK.Connect.Model.Business;
using MPK.Connect.Model.Enums;

namespace MPK.Connect.Model.DataAccess
{
    public class StopTimeInfo
    {
        public StopTimeDto StopTime { get; set; }
        public string RouteId { get; set; }
        public RouteTypes RouteType { get; set; }
    }
}