using MPK.Connect.Service.Business.HarmonySearch.Constants;

namespace MPK.Connect.Service.Business.HarmonySearch
{
    /// <summary>
    /// Stores limits of function argument
    /// </summary>
    public class ArgumentLimit
    {
        public string ArgumentName { get; }
        public double MaxValue { get; }
        public double MinValue { get; }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="argumentName">Name of the argument</param>
        public ArgumentLimit(string argumentName)
        {
            MinValue = -HarmonySearchConstants.DefaultArgumentLimit;
            MaxValue = HarmonySearchConstants.DefaultArgumentLimit;
            ArgumentName = argumentName;
        }

        /// <summary>
        /// The constructor
        /// </summary>
        public ArgumentLimit()
        {
            MinValue = -HarmonySearchConstants.DefaultArgumentLimit;
            MaxValue = HarmonySearchConstants.DefaultArgumentLimit;
            ArgumentName = string.Empty;
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
        /// The constructor
        /// </summary>
        /// <param name="argumentName">Name of the argument</param>
        /// <param name="minValue">Minimum value</param>
        /// <param name="maxValue">Maximum value</param>
        public ArgumentLimit(string argumentName, double maxValue, double minValue)
        {
            ArgumentName = argumentName;
            MaxValue = maxValue;
            MinValue = minValue;
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