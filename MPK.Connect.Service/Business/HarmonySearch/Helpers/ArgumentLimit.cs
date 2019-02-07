using MPK.Connect.Service.Business.HarmonySearch.Constants;

namespace MPK.Connect.Service.Business.HarmonySearch.Helpers
{
    /// <summary>
    /// Stores limits of function argument
    /// </summary>
    public class ArgumentLimit
    {
        public double MaxValue { get; set; }
        public double MinValue { get; set; }

        /// <summary>
        /// The constructor
        /// </summary>
        public ArgumentLimit()
        {
            MinValue = -HarmonySearchConstants.DefaultArgumentLimit;
            MaxValue = HarmonySearchConstants.DefaultArgumentLimit;
        }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="minValue">Minimum value</param>
        /// <param name="maxValue">Maximum value</param>
        public ArgumentLimit(double minValue, double maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        /// <summary>
        /// Checks if number is within the limits
        /// </summary>
        /// <param name="number">Number to check</param>
        /// <returns>True if number is within limits</returns>
        public bool IsWithinLimits(double number)
        {
            return MinValue <= number && number <= MaxValue;
        }
    }
}