using System;
using System.Collections.Generic;
using MPK.Connect.Service.Business.HarmonySearch;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;

namespace MPK.Connect.Test.Service.HarmonySearch
{
    public class DoubleObjectiveFunction : IContinuousObjectiveFunction<double>
    {
        private readonly List<ArgumentLimit> _argumentLimits;
        private readonly IBoundedRandom _random;

        public DoubleObjectiveFunction()
        {
            _random = RandomFactory.GetInstance();
            _argumentLimits = new List<ArgumentLimit>
            {
                new ArgumentLimit(-1, 1),
                new ArgumentLimit(-1, 1)
            };
        }

        public double CalculateObjectiveValue(params double[] arguments)
        {
            // x1^4+x2^4 -0.62*x1^2 - 0.62*x2^3
            var x1 = arguments[0];
            var x2 = arguments[1];
            return Math.Pow(x1, 4) + Math.Pow(x2, 4) - 0.62 * Math.Pow(x1, 2) - 0.62 * Math.Pow(x2, 2);
        }

        public int GetArgumentsCount()
        {
            return 2;
        }

        public double GetArgumentValue(int argumentIndex)
        {
            switch (argumentIndex)
            {
                case 0:
                case 1:
                    return _random.NextValue(GetLowerBound(argumentIndex), GetUpperBound(argumentIndex));

                default:
                    return double.MinValue;
            }
        }

        public double GetLowerBound(int argumentIndex)
        {
            return _argumentLimits[argumentIndex].MinValue;
        }

        public double GetPitchDownAdjustedValue(int argumentIndex, double existingValue)
        {
            var minValue = GetLowerBound(argumentIndex);
            var pitchAdjustment = _random.NextValue(minValue, existingValue);

            return Math.Max(existingValue - pitchAdjustment, minValue);
        }

        public double GetPitchUpAdjustedValue(int argumentIndex, double existingValue)
        {
            var maxValue = GetUpperBound(argumentIndex);
            var pitchAdjustment = _random.NextValue(existingValue, maxValue);

            return Math.Min(existingValue + pitchAdjustment, maxValue);
        }

        public double GetUpperBound(int argumentIndex)
        {
            return _argumentLimits[argumentIndex].MaxValue;
        }

        public bool IsArgumentValuePossible(double argumentValue)
        {
            return true;
        }

        public bool IsArgumentVariable(int argumentIndex)
        {
            return true;
        }

        public bool IsWithinBounds(double value, int argumentIndex)
        {
            return _argumentLimits[argumentIndex].IsWithinLimits(value);
        }

        public void SetLowerBound(int argumentIndex, double value)
        {
            _argumentLimits[argumentIndex].MinValue = value;
        }

        public void SetUpperBound(int argumentIndex, double value)
        {
            _argumentLimits[argumentIndex].MaxValue = value;
        }
    }
}