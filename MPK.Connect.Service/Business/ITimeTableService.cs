using System;
using System.Collections.Generic;
using System.Text;
using MPK.Connect.Model.Business;

namespace MPK.Connect.Service.Business
{
    public interface ITimeTableService
    {
        TimeTable GetTimeTable(string stopId);
    }
}
