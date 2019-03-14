using System;

namespace MPK.Connect.Service.Export
{
    public interface IActionTimer
    {
        TimeSpan MeasureTime(Action action);
    }
}