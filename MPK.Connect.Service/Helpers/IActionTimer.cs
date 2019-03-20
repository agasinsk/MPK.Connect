using System;

namespace MPK.Connect.Service.Helpers
{
    public interface IActionTimer
    {
        TimeSpan MeasureTime(Action action);
    }
}