using System;
using MPK.Connect.Service.Helpers;
using MPK.Connect.Service.Utils;

namespace MPK.Connect.Service.Business.HarmonySearch.Core
{
    /// <inheritdoc/>
    /// <summary>
    /// Represents single harmony (arguments and corresponding function value)
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
        /// The constructor
        /// </summary>
        /// <param name="otherHarmony">Other harmony</param>
        public Harmony(Harmony<T> otherHarmony)
        {
            if (otherHarmony == null) throw new ArgumentNullException(nameof(otherHarmony));

            ObjectiveValue = otherHarmony.ObjectiveValue;
            Arguments = otherHarmony.Arguments;
        }

        /// <summary>
        /// Compares this to other solution by their values
        /// </summary>
        /// <param name="otherHarmony">Other solution</param>
        /// <returns>Comparison result</returns>
        public int CompareTo(Harmony<T> otherHarmony)
        {
            return ObjectiveValue.CompareTo(otherHarmony.ObjectiveValue);
        }

        public override bool Equals(object obj)
        {
            return obj is Harmony<T> harmony && Arguments.Length == harmony.Arguments.Length &&
                   ObjectiveValue.AlmostEquals(harmony.ObjectiveValue);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Arguments.Length, ObjectiveValue);
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

        public override string ToString()
        {
            return $"{nameof(ObjectiveValue)}: {ObjectiveValue}";
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