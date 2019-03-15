using System;
using MPK.Connect.Service.Business.HarmonySearch.Constants;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Generator;

namespace MPK.Connect.Service.Business.HarmonySearch.Core
{
    /// <summary>
    /// Implements harmony search algorithm
    /// </summary>
    public class HarmonySearcher<T> : IHarmonySearcher<T>
    {
        protected readonly IHarmonyGenerator<T> HarmonyGenerator;
        public HarmonyMemory<T> HarmonyMemory { get; }
        public int ImprovisationCount { get; set; }
        public long MaxImprovisationCount { get; }
        public double MaxPitchAdjustmentRatio { get; set; }
        public double MinPitchAdjustmentRatio { get; set; }
        public double PitchAdjustmentRatio { get; set; }
        public bool ShouldImprovePitchAdjustingScenario { get; }
        public double HarmonyMemoryConsiderationRatio => HarmonyGenerator.HarmonyMemoryConsiderationRatio;

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="function">Function to optimize</param>
        public HarmonySearcher(IObjectiveFunction<T> function)
        {
            PitchAdjustmentRatio = HarmonySearchConstants.DefaultPitchAdjustmentRatio;
            MaxImprovisationCount = HarmonySearchConstants.DefaultMaxImprovisationCount;
            HarmonyMemory = new HarmonyMemory<T>(HarmonySearchConstants.DefaultHarmonyMemorySize);

            HarmonyGenerator = HarmonyGeneratorFactory.GetHarmonyGenerator(function, HarmonyMemory,
                HarmonySearchConstants.DefaultHarmonyMemoryConsiderationRatio, PitchAdjustmentRatio);
        }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="function">Function to optimize</param>
        /// <param name="harmonyMemorySize">Harmony memory size</param>
        /// <param name="maxImprovisationCount">Maximum improvisation count</param>
        /// <param name="harmonyMemoryConsiderationRatio">Harmony Memory Consideration Ratio</param>
        /// <param name="pitchAdjustmentRatio">Pitch Adjustment Ratio</param>
        /// <param name="shouldImprovePitchAdjustingScenario">
        /// Enables the improved scenario for PAR
        /// </param>
        /// <param name="minPitchAdjustmentRatio">Minimal Pitch Adjustment Ratio</param>
        /// <param name="maxPitchAdjustmentRatio">Maximal Pitch Adjustment Ratio</param>
        public HarmonySearcher(IObjectiveFunction<T> function, int harmonyMemorySize, long maxImprovisationCount, double harmonyMemoryConsiderationRatio, double pitchAdjustmentRatio, bool shouldImprovePitchAdjustingScenario, double minPitchAdjustmentRatio, double maxPitchAdjustmentRatio)
        {
            MaxImprovisationCount = maxImprovisationCount;
            PitchAdjustmentRatio = pitchAdjustmentRatio;
            HarmonyMemory = new HarmonyMemory<T>(harmonyMemorySize);

            ShouldImprovePitchAdjustingScenario = shouldImprovePitchAdjustingScenario;
            if (shouldImprovePitchAdjustingScenario)
            {
                MinPitchAdjustmentRatio = minPitchAdjustmentRatio;
                MaxPitchAdjustmentRatio = maxPitchAdjustmentRatio;
                PitchAdjustmentRatio = GetCurrentPitchAdjustingRatio(0);
            }

            HarmonyGenerator = HarmonyGeneratorFactory.GetHarmonyGenerator(function, HarmonyMemory, harmonyMemoryConsiderationRatio, PitchAdjustmentRatio);
        }

        /// <summary>
        /// Gets current Pitch Adjustment Ratio for iteration
        /// </summary>
        /// <param name="iterationIndex">Iteration number</param>
        /// <returns>Pitch Adjustment Ratio</returns>
        public double GetCurrentPitchAdjustingRatio(int iterationIndex)
        {
            if (ShouldImprovePitchAdjustingScenario)
            {
                return MaxPitchAdjustmentRatio - (MaxPitchAdjustmentRatio - MinPitchAdjustmentRatio) * iterationIndex / MaxImprovisationCount;
            }

            return PitchAdjustmentRatio;
        }

        public Type GetObjectiveFunctionType()
        {
            return HarmonyGenerator.ObjectiveFunction.GetType();
        }

        /// <summary>
        /// Initializes harmony memory with random solutions
        /// </summary>
        public virtual void InitializeHarmonyMemory()
        {
            for (var i = 0; i < HarmonyMemory.MaxCapacity; i++)
            {
                var randomSolution = HarmonyGenerator.GenerateRandomHarmony();
                HarmonyMemory.Add(randomSolution);
            }
        }

        /// <inheritdoc/>
        /// <summary>
        /// Looks for optimal solution of a function
        /// </summary>
        public virtual Harmony<T> SearchForHarmony()
        {
            InitializeHarmonyMemory();

            ImprovisationCount = 0;
            while (SearchingShouldContinue())
            {
                var worstHarmony = HarmonyMemory.WorstHarmony;
                if (ShouldImprovePitchAdjustingScenario)
                {
                    HarmonyGenerator.PitchAdjustmentRatio = GetCurrentPitchAdjustingRatio(ImprovisationCount);
                }

                var improvisedHarmony = HarmonyGenerator.ImproviseHarmony();
                if (improvisedHarmony.IsBetterThan(worstHarmony) && !HarmonyMemory.Contains(improvisedHarmony))
                {
                    HarmonyMemory.SwapWithWorstHarmony(improvisedHarmony);
                }
                ImprovisationCount++;
            }
            return HarmonyMemory.BestHarmony;
        }

        /// <summary>
        /// Checks if algorithm should continue working
        /// </summary>
        protected bool SearchingShouldContinue()
        {
            return ImprovisationCount < MaxImprovisationCount;
        }
    }
}