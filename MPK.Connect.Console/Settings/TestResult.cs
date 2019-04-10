using System;
using MPK.Connect.Model.Business;
using MPK.Connect.Service.Business.HarmonySearch.Core;

namespace MPK.Connect.TestEnvironment.Settings
{
    public class TestResult
    {
        public Harmony<StopTimeInfo> Solution { get; set; }
        public TimeSpan Time { get; set; }

        public override string ToString()
        {
            return $"{Solution.ObjectiveValue} | {Time.TotalSeconds} s";
        }
    }
}