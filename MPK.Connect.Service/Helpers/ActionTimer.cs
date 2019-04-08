using System;
using System.Diagnostics;

namespace MPK.Connect.Service.Helpers
{
    /// <summary>
    /// The action timer
    /// </summary>
    public class ActionTimer : IActionTimer
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        /// <summary>
        /// Measures execution time of specified action
        /// </summary>
        /// <param name="action">Action to execute</param>
        /// <returns>Elapsed time of action execution</returns>
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