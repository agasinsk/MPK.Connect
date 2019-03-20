using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;

namespace MPK.Connect.Console
{
    public class TestResult
    {
        public HarmonySearchType HarmonySearchType { get; set; }

        public ObjectiveFunctionType ObjectiveFunctionType { get; set; }
        public double ObjectiveFunctionValue { get; set; }
        public double Time { get; set; }

        public object[] GetDataRowParams()
        {
            return new object[]
            {
                HarmonySearchType.ToString(),
                ObjectiveFunctionType.ToString(),
                ObjectiveFunctionValue,
                Time
            };
        }

        public object[] GetDataRowParamsWithoutFunctionType()
        {
            return new object[]
            {
                HarmonySearchType.ToString(),
                ObjectiveFunctionValue,
                Time
            };
        }

        public object[] GetDataRowParamsWithoutSearcherType()
        {
            return new object[]
            {
                ObjectiveFunctionType.ToString(),
                ObjectiveFunctionValue,
                Time
            };
        }
    }
}