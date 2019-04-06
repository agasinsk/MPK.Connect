using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Generator;

namespace MPK.Connect.TestEnvironment.Settings
{
    public class HarmonySearchTestResult
    {
        public double AverageObjectiveFunctionValue { get; set; }
        public double AverageTime { get; set; }
        public double BestObjectiveFunctionValue { get; set; }
        public HarmonyGeneratorType HarmonyGeneratorType { get; set; }
        public HarmonySearchType HarmonySearchType { get; set; }
        public int NonFeasibleCount { get; set; }
        public ObjectiveFunctionType ObjectiveFunctionType { get; set; }
        public int SolutionsCount { get; set; }
        public double WorstObjectiveFunctionValue { get; set; }

        public object[] GetDataRowParams()
        {
            return new object[]
            {
                HarmonySearchType.ToString(),
                HarmonyGeneratorType.ToString(),
                ObjectiveFunctionType.ToString(),
                SolutionsCount,
                NonFeasibleCount,
                AverageObjectiveFunctionValue,
                BestObjectiveFunctionValue,
                WorstObjectiveFunctionValue,
                AverageTime
            };
        }

        public object[] GetDataRowParamsWithoutFunctionType()
        {
            return new object[]
            {
                HarmonySearchType.ToString(),
                SolutionsCount,
                NonFeasibleCount,
                AverageObjectiveFunctionValue,
                BestObjectiveFunctionValue,
                WorstObjectiveFunctionValue,
                AverageTime
            };
        }

        public object[] GetDataRowParamsWithoutSearcherType()
        {
            return new object[]
            {
                HarmonyGeneratorType.ToString(),
                SolutionsCount,
                NonFeasibleCount,
                AverageObjectiveFunctionValue,
                BestObjectiveFunctionValue,
                WorstObjectiveFunctionValue,
                AverageTime
            };
        }
    }
}