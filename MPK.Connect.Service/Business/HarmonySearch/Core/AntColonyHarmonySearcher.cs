using System;
using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Service.Business.HarmonySearch.Constants;
using MPK.Connect.Service.Business.HarmonySearch.Generator;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;
using MPK.Connect.Service.Business.HarmonySearch.ParameterProviders;

namespace MPK.Connect.Service.Business.HarmonySearch.Core
{
    /// <summary>
    /// Harmony search with ant colony optimization incorporated
    /// </summary>
    /// <typeparam name="T">Type of entities</typeparam>
    public class AntColonyHarmonySearcher<T> : HarmonySearcher<T>
    {
        private readonly IAntColonyOptimizer<T> _antColonyOptimizer;

        public override HarmonySearchType Type => HarmonySearchType.AntColony;

        public AntColonyHarmonySearcher(IHarmonyGenerator<T> harmonyGenerator, IParameterProvider parameterProvider, IAntColonyOptimizer<T> antColonyOptimizer, int harmonyMemorySize = HarmonySearchConstants.DefaultHarmonyMemorySize, long maxImprovisationCount = HarmonySearchConstants.DefaultMaxImprovisationCount) : base(harmonyGenerator, parameterProvider, harmonyMemorySize, maxImprovisationCount)
        {
            _antColonyOptimizer = antColonyOptimizer ?? throw new ArgumentNullException(nameof(antColonyOptimizer));
        }

        public override Harmony<T> SearchForHarmony()
        {
            InitializeHarmonyMemory();

            for (var improvisationCount = 0; improvisationCount < MaxImprovisationCount; improvisationCount++)
            {
                var worstHarmony = HarmonyMemory.WorstHarmony;

                // Get parameters
                var harmonyMemoryConsiderationRatio = ParameterProvider.HarmonyMemoryConsiderationRatio;
                var pitchAdjustmentRatio = ParameterProvider.PitchAdjustmentRatio;

                // Improvise harmony with the new parameters
                var improvisedHarmony = HarmonyGenerator.ImproviseHarmony(harmonyMemoryConsiderationRatio, pitchAdjustmentRatio);

                if (improvisedHarmony.IsBetterThan(worstHarmony) && !HarmonyMemory.Contains(improvisedHarmony))
                {
                    HarmonyMemory.SwapWithWorstHarmony(improvisedHarmony);
                }

                _antColonyOptimizer.UpdateGlobalPheromone(HarmonyMemory.BestHarmony);
                var antSolutions = _antColonyOptimizer.GetAntSolutions(HarmonyMemory.MaxCapacity);
                MergeHarmonyMemoryWithAntSolutions(antSolutions);
            }

            return HarmonyMemory.BestHarmony;
        }

        private void MergeHarmonyMemoryWithAntSolutions(IEnumerable<Harmony<T>> antSolutions)
        {
            var allHarmonies = HarmonyMemory.GetAll().Concat(antSolutions).OrderBy(h => h.ObjectiveValue).ToList();

            HarmonyMemory.Clear();

            HarmonyMemory.AddRange(allHarmonies.Take(HarmonyMemory.MaxCapacity));
        }
    }
}