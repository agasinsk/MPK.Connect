using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using MoreLinq.Extensions;

namespace MPK.Connect.Service.Business.HarmonySearch
{
    public class EnhancedHarmonySearcher<T> : HarmonySearcher<T>
    {
        private readonly int _diversityAmount;
        private readonly int _diversityInjectionRate;
        private readonly int _regroupRate;
        private List<HarmonyMemory<T>> _subHarmonyMemories;

        public EnhancedHarmonySearcher(IObjectiveFunction<T> function) : base(function)
        {
        }

        public EnhancedHarmonySearcher(IObjectiveFunction<T> function, int harmonyMemorySize, long maxImprovisationCount, double harmonyMemoryConsiderationRatio, double pitchAdjustmentRatio) : base(function, harmonyMemorySize, maxImprovisationCount, harmonyMemoryConsiderationRatio, pitchAdjustmentRatio)
        {
        }

        public EnhancedHarmonySearcher(IObjectiveFunction<T> function, int harmonyMemorySize, long maxImprovisationCount, double harmonyMemoryConsiderationRatio, double minPitchAdjustmentRatio, double maxPitchAdjustmentRatio) : base(function, harmonyMemorySize, maxImprovisationCount, harmonyMemoryConsiderationRatio, minPitchAdjustmentRatio, maxPitchAdjustmentRatio)
        {
        }

        public EnhancedHarmonySearcher(IObjectiveFunction<T> function, int harmonyMemorySize, long maxImprovisationCount, double harmonyMemoryConsiderationRatio, bool shouldImprovePitchAdjustingScenario) : base(function, harmonyMemorySize, maxImprovisationCount, harmonyMemoryConsiderationRatio, shouldImprovePitchAdjustingScenario)
        {
        }

        public EnhancedHarmonySearcher(IObjectiveFunction<T> function, bool shouldImprovePitchAdjustingScenario) : base(function, shouldImprovePitchAdjustingScenario)
        {
        }

        /// <inheritdoc/>
        /// <summary>
        /// Initializes a collection of sub-harmony memories with random solution
        /// </summary>
        public override void InitializeHarmonyMemory()
        {
            var subHarmonyMemoriesCount = (int)Math.Ceiling((decimal)HarmonyMemory.MaxCapacity / 4);
            _subHarmonyMemories = new List<HarmonyMemory<T>>(4);
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

                if (improvisationCount % _regroupRate == 0)
                {
                    RegroupHarmonyMemories();
                }

                var worstHarmony = HarmonyMemory.WorstHarmony;
                if (ShouldImprovePitchAdjustingScenario)
                {
                    HarmonyGenerator.PitchAdjustmentRatio = GetCurrentPitchAdjustingRatio(improvisationCount);
                }

                foreach (var subHarmonyMemory in _subHarmonyMemories)
                {
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
            throw new NotImplementedException();
        }

        private void RegroupHarmonyMemories()
        {
            throw new NotImplementedException();
        }
    }
}