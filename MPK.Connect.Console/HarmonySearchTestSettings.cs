using System;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using static MPK.Connect.Service.Business.HarmonySearch.Constants.HarmonySearchConstants;

namespace MPK.Connect.Console
{
    public class HarmonySearchTestSettings<T> where T : class
    {
        public double HarmonyMemoryConsiderationRatio { get; set; }

        public int HarmonyMemorySize { get; set; }

        public Type HarmonySearcherType { get; set; }

        public bool ImprovedPitchAdjustingScenario { get; set; }

        public int MaxImprovisationCount { get; set; }

        public double? MaxPitchAdjustingRatio { get; set; }

        public double? MinPitchAdjustingRatio { get; set; }

        public Type ObjectiveFunctionType { get; set; }

        public double PitchAdjustingRatio { get; set; }

        public HarmonySearchTestSettings()
        {
            MaxImprovisationCount = DefaultMaxImprovisationCount;
            HarmonyMemorySize = DefaultHarmonyMemorySize;
            HarmonyMemoryConsiderationRatio = DefaultHarmonyMemoryConsiderationRatio;
            PitchAdjustingRatio = DefaultPitchAdjustmentRatio;
            MinPitchAdjustingRatio = DefaultMinPitchAdjustmentRatio;
            MaxPitchAdjustingRatio = DefaultMaxPitchAdjustmentRatio;
        }

        public IHarmonySearcher<T> GetHarmonySearcher(Graph<int, StopTimeInfo> graph, Location source,
            Location destination)
        {
            var objectiveFunction = Activator.CreateInstance(ObjectiveFunctionType, graph, source, destination) as IObjectiveFunction<T>;

            if (HarmonySearcherType == typeof(HarmonySearcher<T>))
            {
                return Activator.CreateInstance(HarmonySearcherType, objectiveFunction, HarmonyMemorySize, MaxImprovisationCount, HarmonyMemoryConsiderationRatio, PitchAdjustingRatio) as IHarmonySearcher<T>;
            }

            if (HarmonySearcherType == typeof(ImprovedHarmonySearcher<T>))
            {
                return Activator.CreateInstance(HarmonySearcherType, objectiveFunction, HarmonyMemorySize, MaxImprovisationCount, HarmonyMemoryConsiderationRatio, MinPitchAdjustingRatio, MaxPitchAdjustingRatio) as IHarmonySearcher<T>;
            }

            if (HarmonySearcherType == typeof(DividedHarmonySearcher<T>))
            {
                return Activator.CreateInstance(HarmonySearcherType, objectiveFunction, HarmonyMemorySize, MaxImprovisationCount, HarmonyMemoryConsiderationRatio, PitchAdjustingRatio) as IHarmonySearcher<T>;
            }

            if (HarmonySearcherType == typeof(DynamicHarmonySearcher<T>))
            {
                return Activator.CreateInstance(HarmonySearcherType, objectiveFunction, HarmonyMemorySize, MaxImprovisationCount) as IHarmonySearcher<T>;
            }

            var harmonySearcher =
                Activator.CreateInstance(HarmonySearcherType, objectiveFunction, HarmonyMemorySize, MaxImprovisationCount, HarmonyMemoryConsiderationRatio, PitchAdjustingRatio, ImprovedPitchAdjustingScenario, MinPitchAdjustingRatio, MaxPitchAdjustingRatio) as IHarmonySearcher<T>;

            return harmonySearcher;
        }
    }
}