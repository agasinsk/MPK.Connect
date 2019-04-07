using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Business.HarmonySearch.Constants;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Generator;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;
using MPK.Connect.TestEnvironment.Factories;

namespace MPK.Connect.TestEnvironment.Settings
{
    public class HarmonySearchTestSettings
    {
        public HarmonyGeneratorType HarmonyGeneratorType { get; set; }
        public double HarmonyMemoryConsiderationRatio { get; set; }

        public int HarmonyMemorySize { get; set; }

        public HarmonySearchType HarmonySearcherType { get; set; }

        public int MaxImprovisationCount { get; set; }

        public double MaxPitchAdjustingRatio { get; set; }

        public double MinPitchAdjustingRatio { get; set; }

        public ObjectiveFunctionType ObjectiveFunctionType { get; set; }

        public double PitchAdjustingRatio { get; set; }

        public HarmonySearchTestSettings()
        {
            MaxImprovisationCount = HarmonySearchConstants.DefaultMaxImprovisationCount;
            HarmonyMemorySize = HarmonySearchConstants.DefaultHarmonyMemorySize;
            HarmonyMemoryConsiderationRatio = HarmonySearchConstants.DefaultHarmonyMemoryConsiderationRatio;
            PitchAdjustingRatio = HarmonySearchConstants.DefaultPitchAdjustmentRatio;
            MinPitchAdjustingRatio = HarmonySearchConstants.DefaultMinPitchAdjustmentRatio;
            MaxPitchAdjustingRatio = HarmonySearchConstants.DefaultMaxPitchAdjustmentRatio;
        }

        public IHarmonySearcher<StopTimeInfo> GetHarmonySearcher(Graph<int, StopTimeInfo> graph, Location source,
            Location destination)
        {
            var objectiveFunction = ObjectiveFunctionFactory.GetInstance(ObjectiveFunctionType, destination);

            var harmonyGenerator = HarmonyGeneratorFactory.GetInstance(HarmonyGeneratorType, objectiveFunction, graph, source, destination);

            var antColonyOptimizer = new StopTimeAntColonyOptimizer(objectiveFunction, graph, source, destination);

            var harmonySearcher = HarmonySearcherFactory.GetInstance(HarmonySearcherType, harmonyGenerator,
                HarmonyMemorySize, MaxImprovisationCount, HarmonyMemoryConsiderationRatio, PitchAdjustingRatio,
                MinPitchAdjustingRatio, MaxPitchAdjustingRatio, antColonyOptimizer);

            return harmonySearcher;
        }
    }
}