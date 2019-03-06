using System;
using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Helpers;
using static MPK.Connect.Service.Business.HarmonySearch.Constants.HarmonySearchConstants;

namespace MPK.Connect.Service.Business.HarmonySearch.Core
{
    public class GroupedHarmonySearcher<T> : HarmonySearcher<T>
    {
        private List<HarmonyMemory<T>> _subHarmonyMemories;
        private long RegroupRate => MaxImprovisationCount / 10;

        public GroupedHarmonySearcher(IObjectiveFunction<T> function) : base(function)
        {
        }

        public GroupedHarmonySearcher(IObjectiveFunction<T> function, int harmonyMemorySize = DefaultHarmonyMemorySize, long maxImprovisationCount = DefaultMaxImprovisationCount, double harmonyMemoryConsiderationRatio = DefaultHarmonyMemoryConsiderationRatio, double pitchAdjustmentRatio = DefaultPitchAdjustmentRatio) : base(function, harmonyMemorySize, maxImprovisationCount, harmonyMemoryConsiderationRatio, pitchAdjustmentRatio)
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
                while (subHarmonyMemory.Count < subHarmonyMemory.MaxCapacity)
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
                if (improvisationCount > 0 && improvisationCount % RegroupRate == 0)
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

        /// <summary>
        /// Gets best harmony from sub Harmony memories
        /// </summary>
        /// <returns>Best harmony</returns>
        private Harmony<T> GetBestHarmony()
        {
            return _subHarmonyMemories.Select(x => x.BestHarmony).OrderByDescending(h => h.ObjectiveValue).First();
        }

        /// <summary>
        /// Regroups sub Harmony Memories
        /// </summary>
        private void RegroupHarmonyMemories()
        {
            var allSolutions = _subHarmonyMemories.SelectMany(h => h.GetAll()).ToList();
            _subHarmonyMemories.ForEach(h => h.Clear());

            foreach (var subHarmonyMemory in _subHarmonyMemories)
            {
                while (subHarmonyMemory.Count < subHarmonyMemory.MaxCapacity)
                {
                    var randomSolution = allSolutions.GetRandomElement();
                    subHarmonyMemory.Add(randomSolution);
                    allSolutions.Remove(randomSolution);
                }
            }
        }
    }
}