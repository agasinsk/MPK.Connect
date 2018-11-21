using System.Collections.Generic;

namespace MPK.Connect.Model.Business
{
    public class DirectionStopTimes
    {
        public string Direction { get; set; }
        public List<StopTimeCore> StopTimes { get; set; }
    }
}