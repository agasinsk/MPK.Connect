using System;
using System.Diagnostics;

namespace MPK.Connect.Service.Helpers
{
    public class ActionTimer : IActionTimer
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public TimeSpan MeasureTime(Action action)
        {
            _stopwatch.Reset();
            _stopwatch.Start();

            action.Invoke();

            _stopwatch.Stop();

            return _stopwatch.Elapsed;
        }
    }
}