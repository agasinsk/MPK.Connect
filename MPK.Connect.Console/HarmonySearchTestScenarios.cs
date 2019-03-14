using System.Collections.Generic;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;

namespace MPK.Connect.Console
{
    public class HarmonySearchTestScenarios<T> where T : class

    {
        public List<HarmonySearchTestSettings<T>> Settings { get; set; }

        public HarmonySearchTestScenarios()
        {
            Settings = new List<HarmonySearchTestSettings<T>>
        {
            //////////////////////// RandomStopTimeObjectiveFunction
            new HarmonySearchTestSettings<T> // standard, constant PAR
            {
                HarmonySearcherType = typeof(HarmonySearcher<T>),
                ObjectiveFunctionType = typeof(RandomStopTimeObjectiveFunction),
            },
            new HarmonySearchTestSettings<T> // standard, dynamic PAR
            {
                HarmonySearcherType = typeof(HarmonySearcher<T>),
                ObjectiveFunctionType = typeof(RandomStopTimeObjectiveFunction),
                ImprovedPitchAdjustingScenario = true
            },
            new HarmonySearchTestSettings<T> // subHMs, constant PAR
            {
                HarmonySearcherType = typeof(DividedHarmonySearcher<T>),
                ObjectiveFunctionType = typeof(RandomStopTimeObjectiveFunction)
            },
            new HarmonySearchTestSettings<T> // subHMs, dynamic PAR
            {
                HarmonySearcherType = typeof(DividedHarmonySearcher<T>),
                ObjectiveFunctionType = typeof(RandomStopTimeObjectiveFunction),
                ImprovedPitchAdjustingScenario = true
            },
            //////////////////////// RandomStopObjectiveFunction
            new HarmonySearchTestSettings<T> // standard, constant PAR
            {
                HarmonySearcherType = typeof(HarmonySearcher<T>),
                ObjectiveFunctionType = typeof(RandomStopObjectiveFunction)
            },
            new HarmonySearchTestSettings<T> // standard, dynamic PAR
            {
                HarmonySearcherType = typeof(HarmonySearcher<T>),
                ObjectiveFunctionType = typeof(RandomStopObjectiveFunction),
                ImprovedPitchAdjustingScenario = true
            },
            new HarmonySearchTestSettings<T> // subHMs, constant PAR
            {
                HarmonySearcherType = typeof(DividedHarmonySearcher<T>),
                ObjectiveFunctionType = typeof(RandomStopObjectiveFunction)
            },
            new HarmonySearchTestSettings<T> // subHMs, dynamic PAR
            {
                HarmonySearcherType = typeof(DividedHarmonySearcher<T>),
                ObjectiveFunctionType = typeof(RandomStopObjectiveFunction),
                ImprovedPitchAdjustingScenario = true
            },
            //////////////////////// DistanceStopTimeObjectiveFunction
            new HarmonySearchTestSettings<T> // standard, constant PAR
            {
                HarmonySearcherType = typeof(HarmonySearcher<T>),
                ObjectiveFunctionType = typeof(DistanceStopTimeObjectiveFunction)
            },
            new HarmonySearchTestSettings<T> // standard, dynamic PAR
            {
                HarmonySearcherType = typeof(HarmonySearcher<T>),
                ObjectiveFunctionType = typeof(DistanceStopTimeObjectiveFunction),
                ImprovedPitchAdjustingScenario = true
            },
            new HarmonySearchTestSettings<T> // subHMs, constant PAR
            {
                HarmonySearcherType = typeof(DividedHarmonySearcher<T>),
                ObjectiveFunctionType = typeof(DistanceStopTimeObjectiveFunction)
            },
            new HarmonySearchTestSettings<T> // subHMs, dynamic PAR
            {
                HarmonySearcherType = typeof(DividedHarmonySearcher<T>),
                ObjectiveFunctionType = typeof(DistanceStopTimeObjectiveFunction),
                ImprovedPitchAdjustingScenario = true
            }
        };
        }
    }
}