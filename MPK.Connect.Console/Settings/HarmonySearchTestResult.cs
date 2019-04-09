using System;
using MPK.Connect.Model.Business;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Generator;

namespace MPK.Connect.TestEnvironment.Settings
{
    public class HarmonySearchTestResult
    {
        public Harmony<StopTimeInfo> BestHarmony { get; set; }
        public HarmonyGeneratorType HarmonyGeneratorType { get; set; }
        public HarmonySearchType HarmonySearchType { get; set; }
        public int ImprovisationCount { get; set; }
        public int Iteration { get; set; }
        public ObjectiveFunctionType ObjectiveFunctionType { get; set; }
        public TimeSpan Time { get; set; }
    }
}