using System;
using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using static MPK.Connect.Service.Business.HarmonySearch.Constants.HarmonySearchConstants;

namespace MPK.Connect.Service.Business.HarmonySearch.Core
{
    public class GeneralHarmonySearcher<T> : HarmonySearcher<T>
    {
        private readonly int _diversityAmount;
        private readonly int _diversityInjectionRate;
        private List<HarmonyMemory<T>> _subHarmonyMemories;
        private long RegroupRate => MaxImprovisationCount / 10;

        public GeneralHarmonySearcher(IObjectiveFunction<T> function) : base(function)
        {
        }

        public GeneralHarmonySearcher(IObjectiveFunction<T> function, int harmonyMemorySize = DefaultHarmonyMemorySize, long maxImprovisationCount = DefaultMaxImprovisationCount, double harmonyMemoryConsiderationRatio = DefaultHarmonyMemoryConsiderationRatio, double pitchAdjustmentRatio = DefaultPitchAdjustmentRatio) : base(function, harmonyMemorySize, maxImprovisationCount, harmonyMemoryConsiderationRatio, pitchAdjustmentRatio)
        {
        }

        public GeneralHarmonySearcher(IObjectiveFunction<T> function, bool shouldImprovePitchAdjustingScenario = false) : base(function, shouldImprovePitchAdjustingScenario)
        {
        }

        /// <inheritdoc/>
        /// <summary>
        /// Initializes a collection of sub-harmony memories with random solutions
        /// </summary>
        public override void InitializeHarmonyMemory()
        {
            _subHarmonyMemories = new List<HarmonyMemory<T>>(4);
            var subHarmonyMemoriesCount = (int)Math.Ceiling((decimal)HarmonyMemory.MaxCapacity / 4);
            for (var i = 0; i < _subHarmonyMemories.Capacity; i++)
            {
                _subHarmonyMemories.Add(new HarmonyMemory<T>(subHarmonyMemoriesCount));
            }

            foreach (var subHarmonyMemory in _subHarmonyMemories)
            {
                for (var i = 0; i < subHarmonyMemory.MaxCapacity; i++)
                {
                    var randomHarmony = HarmonyGenerator.GenerateRandomHarmony();
                    subHarmonyMemory.Add(randomHarmony);
                }
            }
        }

        public override Harmony<T> SearchForHarmony()
        {
            InitializeHarmonyMemory();

            for (var improvisationCount = 0; improvisationCount < MaxImprovisationCount; improvisationCount++)
            {
                //if (improvisationCount % _diversityInjectionRate == 0)
                //{
                //    IntroduceDiversity(_diversityAmount);
                //}

                if (improvisationCount % RegroupRate == 0)
                {
                    RegroupHarmonyMemories();
                }

                foreach (var subHarmonyMemory in _subHarmonyMemories)
                {
                    HarmonyGenerator.HarmonyMemory = subHarmonyMemory;

                    var worstHarmony = subHarmonyMemory.WorstHarmony;
                    if (ShouldImprovePitchAdjustingScenario)
                    {
                        HarmonyGenerator.PitchAdjustmentRatio = GetCurrentPitchAdjustingRatio(improvisationCount);
                    }
                    var improvisedHarmony = HarmonyGenerator.ImproviseHarmony();
                    if (improvisedHarmony.IsBetterThan(worstHarmony) && !subHarmonyMemory.Contains(improvisedHarmony))
                    {
                        subHarmonyMemory.SwapWithWorstHarmony(improvisedHarmony);
                    }
                }
            }

            return GetBestHarmony();
        }

        private Harmony<T> GetBestHarmony()
        {
            return _subHarmonyMemories.Select(x => x.BestHarmony).OrderByDescending(h => h.ObjectiveValue).First();
        }

        private void IntroduceDiversity(int diversityAmount)
        {
        }

        private void RegroupHarmonyMemories()
        {
        }
    }
}