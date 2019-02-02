using MPK.Connect.Service.Business.HarmonySearch;

namespace MPK.Connect.Test.Service.HarmonySearch
{
    public class DoubleCalculator : ICalculator<double>
    {
        public double Add(double firstValue, double secondValue)
        {
            return firstValue + secondValue;
        }

        public double Divide(double firstValue, double secondValue)
        {
            return firstValue / secondValue;
        }

        public double Multiply(double firstValue, double secondValue)
        {
            return firstValue * secondValue;
        }

        public double Subtract(double firstValue, double secondValue)
        {
            return firstValue - secondValue;
        }
    }
}