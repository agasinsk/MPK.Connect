using System;

namespace MPK.Connect.Service.Helpers
{
    /// <summary>
    /// Interface for action timer
    /// </summary>
    public interface IActionTimer
    {
        /// <summary>
        /// Measures execution time of specified action
        /// </summary>
        /// <param name="action">Action to execute</param>
        /// <returns>Elapsed time of action execution</returns>
        TimeSpan MeasureTime(Action action);
    }
}