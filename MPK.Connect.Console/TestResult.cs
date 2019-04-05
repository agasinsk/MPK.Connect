using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Generator;

namespace MPK.Connect.Console
{
    public class TestResult
    {
        public HarmonyGeneratorType HarmonyGeneratorType { get; set; }
        public HarmonySearchType HarmonySearchType { get; set; }

        public int NonFeasibleCount { get; set; }
        public double ObjectiveFunctionValue { get; set; }
        public int SolutionsCount { get; set; }
        public double Time { get; set; }

        public object[] GetDataRowParams()
        {
            return new object[]
            {
                HarmonySearchType.ToString(),
                HarmonyGeneratorType.ToString(),
                SolutionsCount,
                NonFeasibleCount,
                ObjectiveFunctionValue,
                Time
            };
        }

        public object[] GetDataRowParamsWithoutFunctionType()
        {
            return new object[]
            {
                HarmonySearchType.ToString(),
                SolutionsCount,
                NonFeasibleCount,
                ObjectiveFunctionValue,
                Time
            };
        }

        public object[] GetDataRowParamsWithoutSearcherType()
        {
            return new object[]
            {
                HarmonyGeneratorType.ToString(),
                SolutionsCount,
                NonFeasibleCount,
                ObjectiveFunctionValue,
                Time
            };
        }
    }
}