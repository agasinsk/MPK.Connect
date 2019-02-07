using System;

namespace MPK.Connect.Service.Business.HarmonySearch
{
    /// <inheritdoc/>
    /// <summary>
    /// Represents single problem harmony (arguments and corresponding function value)
    /// </summary>
	public class Harmony<T> : IComparable<Harmony<T>>
    {
        public T[] Arguments;
        public double ObjectiveValue;

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="objectiveValue">Solution value</param>
        /// <param name="arguments">Solution arguments</param>
        public Harmony(double objectiveValue, params T[] arguments)
        {
            ObjectiveValue = objectiveValue;
            Arguments = arguments;
        }

        /// <summary>
        /// Compares this to other solution by their values
        /// </summary>
        /// <param name="otherHarmony">Other solution</param>
        /// <returns>Comparison result</returns>
        public int CompareTo(Harmony<T> otherHarmony)
        {
            var objectiveValueComparison = ObjectiveValue.CompareTo(otherHarmony.ObjectiveValue);
            return objectiveValueComparison;
        }

        /// <summary>
        /// Determines if solution is better than other solution
        /// </summary>
        /// <param name="otherHarmony">Other solution</param>
        /// <returns>If solution is better</returns>
        public bool IsBetterThan(Harmony<T> otherHarmony)
        {
            return ObjectiveValue < otherHarmony.ObjectiveValue;
        }

        /// <summary>
        /// Gets argument by index
        /// </summary>
        /// <param name="argumentIndex">Index of argument</param>
        /// <returns>Argument at index</returns>
        internal T GetArgument(int argumentIndex)
        {
            if (argumentIndex > Arguments.Length)
            {
                throw new IndexOutOfRangeException();
            }

            return Arguments[argumentIndex];
        }
    }
}