using MPK.Connect.Service.Business.HarmonySearch.Generator;
using MPK.Connect.Service.Business.HarmonySearch.ParameterProviders;
using MPK.Connect.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public override HarmonySearchType Type => GetHarmonySearchType();

        private long RegroupRate => MaxImprovisationCount / 10;

        /// <summary>
        /// Initializes a new instance of the <see cref="DividedHarmonySearcher{T}"/> class.
        /// </summary>
        /// <param name="harmonyGenerator">Harmony generator</param>
        /// <param name="parameterProvider"></param>
        /// <param name="harmonyMemorySize">Harmony memory size</param>
        /// <param name="maxImprovisationCount">Maximal improvisation count</param>
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
                var triesCount = 0;
                while (subHarmonyMemory.Count < subHarmonyMemory.MaxCapacity && triesCount < subHarmonyMemory.MaxCapacity * 3)
                {
                    var randomHarmony = HarmonyGenerator.GenerateRandomHarmony();
                    subHarmonyMemory.Add(randomHarmony);
                    triesCount++;
                }
            }
        }

        /// <summary>
        /// Looks for optimal solution of a function
        /// </summary>
        /// <returns></returns>
        /// <inheritdoc/>
        public override Harmony<T> SearchForHarmony()
        {
            InitializeHarmonyMemory();

            ImprovisationCount = 0;
            ImprovisationCountWithTheSameBestValue = 0;

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
        /// Established harmony search type
        /// </summary>
        /// <returns></returns>
        private HarmonySearchType GetHarmonySearchType()
        {
            switch (ParameterProvider)
            {
                case DynamicPitchAdjustmentRatioProvider _:
                    return HarmonySearchType.ImprovedDivided;

                case DynamicParameterProvider _:
                    return HarmonySearchType.DynamicDivided;

                default:
                    return HarmonySearchType.Divided;
            }
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