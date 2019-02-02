using System;
using MPK.Connect.Service.Business.HarmonySearch;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;

namespace MPK.Connect.Test.Service.HarmonySearch
{
    public class DoubleObjectiveFunction : DoubleCalculator, IObjectiveFunction<double>
    {
        private readonly IRandomGenerator<double> _random = new RandomGenerator();

        public double CalculateObjectiveValue(params double[] arguments)
        {
            // x1^2 + x1*x2;
            return Math.Pow(arguments[0], 2) + arguments[0] * arguments[1];
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
            throw new System.NotImplementedException();
        }

        public double GetLowerBound(int argumentIndex)
        {
            switch (argumentIndex)
            {
                case 0:
                    return -10;

                case 1:
                    return -10;

                default:
                    return double.MinValue;
            }
        }

        public double GetMaximumContinuousPitchAdjustmentProportion()
        {
            throw new System.NotImplementedException();
        }

        public int GetMaximumDiscretePitchAdjustmentIndex()
        {
            throw new System.NotImplementedException();
        }

        public int GetPossibleDiscreteValuesCount(int argumentIndex)
        {
            throw new System.NotImplementedException();
        }

        public double GetUpperBound(int argumentIndex)
        {
            switch (argumentIndex)
            {
                case 0:
                    return 10;

                case 1:
                    return 10;

                default:
                    return double.MaxValue;
            }
        }

        public bool IsArgumentDiscrete(int argumentIndex)
        {
            return false;
        }

        public bool IsArgumentVariable(int argumentIndex)
        {
            return true;
        }

        public bool IsWithinBounds(double value, int argumentIndex)
        {
            return value < GetUpperBound(argumentIndex) && value > GetLowerBound(argumentIndex);
        }
    }
}