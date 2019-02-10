using System;
using System.Collections.Generic;
using MPK.Connect.Service.Business.HarmonySearch;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;

namespace MPK.Connect.Test.Service.HarmonySearch
{
    public class DoubleObjectiveFunction : DoubleCalculator, IObjectiveFunction<double>
    {
        private readonly List<ArgumentLimit> _argumentLimits;
        private readonly IRandomGenerator<double> _random = new RandomGenerator();

        public DoubleObjectiveFunction()
        {
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

        public double GetArgumentValue(int argumentIndex, int? discreteValueIndex = null)
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

        public int GetIndexOfDiscreteValue(int argumentIndex, double argumentValue)
        {
            throw new NotImplementedException();
        }

        public double GetLowerBound(int argumentIndex)
        {
            return _argumentLimits[argumentIndex].MinValue;
        }

        public double GetMaximumContinuousPitchAdjustmentProportion()
        {
            throw new NotImplementedException();
        }

        public int GetMaximumDiscretePitchAdjustmentIndex()
        {
            throw new NotImplementedException();
        }

        public int GetPossibleDiscreteValuesCount(int argumentIndex)
        {
            throw new NotImplementedException();
        }

        public double GetUpperBound(int argumentIndex)
        {
            return _argumentLimits[argumentIndex].MaxValue;
        }

        public bool IsArgumentDiscrete(int argumentIndex)
        {
            return false;
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

        public void SaveArgumentValue(int argumentIndex, double argumentValue)
        {
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