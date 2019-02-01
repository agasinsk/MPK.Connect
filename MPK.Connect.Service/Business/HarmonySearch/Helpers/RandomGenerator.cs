using System;

namespace MPK.Connect.Service.Business.HarmonySearch.Helpers
{
    /// <inheritdoc/>
    /// <summary>
    /// Random number generator
    /// </summary>
    public class RandomGenerator : Random, IRandomGenerator<double>
    {
        /// <summary>
        /// Generates a double number bounded by two values
        /// </summary>
        /// <param name="minValue">Minimum value</param>
        /// <param name="maxValue">Maximum value</param>
        /// <returns>Bounded double number</returns>
        public double Next(double minValue, double maxValue)
        {
            var randomDouble = NextDouble();
            randomDouble = randomDouble * (maxValue - minValue) + minValue;
            // correct for rounding
            if (randomDouble >= maxValue)
            {
                randomDouble = Math.Floor(maxValue);
            }
            return randomDouble;
        }
    }
}