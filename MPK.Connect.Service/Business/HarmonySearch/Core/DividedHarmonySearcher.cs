using System;
using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Service.Business.HarmonySearch.Generator;
using MPK.Connect.Service.Business.HarmonySearch.ParameterProviders;
using MPK.Connect.Service.Utils;
using static MPK.Connect.Service.Business.HarmonySearch.Constants.HarmonySearchConstants;

namespace MPK.Connect.Service.Business.HarmonySearch.Core
{
    /// <summary>
    /// Implements Harmony Search with sub-populations
    /// </summary>
    /// <typeparam name="T">Type of solution elements</typeparam>
    public class DividedHarmonySearcher<T> : HarmonySearcher<T>
    {
        private List<HarmonyMemory<T>> _subHarmonyMemories;
        public override HarmonySearchType Type => HarmonySearchType.Divided;
        private long RegroupRate => MaxImprovisationCount / 10;

        public DividedHarmonySearcher(IHarmonyGenerator<T> harmonyGenerator, IParameterProvider parameterProvider, int harmonyMemorySize = DefaultHarmonyMemorySize, long maxImprovisationCount = DefaultMaxImprovisationCount) : base(harmonyGenerator, parameterProvider, harmonyMemorySize, maxImprovisationCount)
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

            while (SearchingShouldContinue())
            {
                if (ImprovisationCount > 0 && ImprovisationCount % RegroupRate == 0)
                {
                    RegroupHarmonyMemories();
                }

                foreach (var subHarmonyMemory in _subHarmonyMemories)
                {
                    // Set the right harmony memory
                    HarmonyGenerator.HarmonyMemory = subHarmonyMemory;

                    var harmonyMemoryConsiderationRatio = ParameterProvider.HarmonyMemoryConsiderationRatio;
                    var pitchAdjustmentRatio = ParameterProvider.PitchAdjustmentRatio;

                    var improvisedHarmony = HarmonyGenerator.ImproviseHarmony(harmonyMemoryConsiderationRatio, pitchAdjustmentRatio);

                    var worstHarmony = subHarmonyMemory.WorstHarmony;

                    if (improvisedHarmony.IsBetterThan(worstHarmony) && !subHarmonyMemory.Contains(improvisedHarmony))
                    {
                        subHarmonyMemory.SwapWithWorstHarmony(improvisedHarmony);
                    }

                    SaveBestHarmony(GetBestHarmony());

                    ImprovisationCount++;
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
            return _subHarmonyMemories.Select(x => x.BestHarmony)
                .OrderBy(h => h.ObjectiveValue)
                .First();
        }

        /// <summary>
        /// Regroups sub Harmony Memories
        /// </summary>
        private void RegroupHarmonyMemories()
        {
            var allSolutions = _subHarmonyMemories.SelectMany(h => h.GetAll()).ToList();
            var solutionIds = Enumerable.Range(0, allSolutions.Count).ToList();

            foreach (var subHarmonyMemory in _subHarmonyMemories)
            {
                subHarmonyMemory.Clear();

                for (var index = 0; index < subHarmonyMemory.MaxCapacity; index++)
                {
                    var randomIndex = solutionIds.GetRandomElement();
                    var randomSolution = allSolutions.ElementAt(randomIndex);

                    if (subHarmonyMemory.Add(new Harmony<T>(randomSolution)))
                    {
                        solutionIds.Remove(randomIndex);
                    }
                }
            }

            foreach (var subHarmonyMemory in _subHarmonyMemories.Where(hm => hm.Count < hm.MaxCapacity))
            {
                while (subHarmonyMemory.Count < subHarmonyMemory.MaxCapacity)
                {
                    var randomHarmony = HarmonyGenerator.GenerateRandomHarmony();
                    subHarmonyMemory.Add(randomHarmony);
                }
            }
        }
    }
}