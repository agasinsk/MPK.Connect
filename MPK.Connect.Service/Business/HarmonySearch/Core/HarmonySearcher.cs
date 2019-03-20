using System;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Generator;
using static MPK.Connect.Service.Business.HarmonySearch.Constants.HarmonySearchConstants;

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
        public ObjectiveFunctionType ObjectiveFunctionType => HarmonyGenerator.ObjectiveFunction.Type;
        public double PitchAdjustmentRatio { get; set; }

        public virtual HarmonySearchType Type => HarmonySearchType.Standard;

        public double HarmonyMemoryConsiderationRatio => HarmonyGenerator.HarmonyMemoryConsiderationRatio;

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="function">Function to optimize</param>
        public HarmonySearcher(IObjectiveFunction<T> function)
        {
            PitchAdjustmentRatio = DefaultPitchAdjustmentRatio;
            MaxImprovisationCount = DefaultMaxImprovisationCount;
            HarmonyMemory = new HarmonyMemory<T>(DefaultHarmonyMemorySize);

            HarmonyGenerator = HarmonyGeneratorFactory.GetHarmonyGenerator(function, HarmonyMemory,
                DefaultHarmonyMemoryConsiderationRatio, PitchAdjustmentRatio);
        }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="function">Function to optimize</param>
        /// <param name="harmonyMemorySize">Harmony memory size</param>
        /// <param name="maxImprovisationCount">Maximum improvisation count</param>
        public HarmonySearcher(IObjectiveFunction<T> function, int harmonyMemorySize, long maxImprovisationCount)
        {
            PitchAdjustmentRatio = DefaultPitchAdjustmentRatio;
            MaxImprovisationCount = maxImprovisationCount;
            HarmonyMemory = new HarmonyMemory<T>(harmonyMemorySize);

            HarmonyGenerator = HarmonyGeneratorFactory.GetHarmonyGenerator(function, HarmonyMemory,
                DefaultHarmonyMemoryConsiderationRatio, PitchAdjustmentRatio);
        }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="function">Function to optimize</param>
        /// <param name="harmonyMemorySize">Harmony memory size</param>
        /// <param name="maxImprovisationCount">Maximum improvisation count</param>
        /// <param name="harmonyMemoryConsiderationRatio">Harmony Memory Consideration Ratio</param>
        public HarmonySearcher(IObjectiveFunction<T> function, int harmonyMemorySize, long maxImprovisationCount, double harmonyMemoryConsiderationRatio)
        {
            MaxImprovisationCount = maxImprovisationCount;
            PitchAdjustmentRatio = DefaultPitchAdjustmentRatio;
            HarmonyMemory = new HarmonyMemory<T>(harmonyMemorySize);

            HarmonyGenerator = HarmonyGeneratorFactory.GetHarmonyGenerator(function, HarmonyMemory, harmonyMemoryConsiderationRatio, PitchAdjustmentRatio);
        }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="function">Function to optimize</param>
        /// <param name="harmonyMemorySize">Harmony memory size</param>
        /// <param name="maxImprovisationCount">Maximum improvisation count</param>
        /// <param name="harmonyMemoryConsiderationRatio">Harmony Memory Consideration Ratio</param>
        /// <param name="pitchAdjustmentRatio">Pitch Adjustment Ratio</param>
        public HarmonySearcher(IObjectiveFunction<T> function, int harmonyMemorySize, long maxImprovisationCount, double harmonyMemoryConsiderationRatio, double pitchAdjustmentRatio)
        {
            MaxImprovisationCount = maxImprovisationCount;
            PitchAdjustmentRatio = pitchAdjustmentRatio;
            HarmonyMemory = new HarmonyMemory<T>(harmonyMemorySize);

            HarmonyGenerator = HarmonyGeneratorFactory.GetHarmonyGenerator(function, HarmonyMemory, harmonyMemoryConsiderationRatio, PitchAdjustmentRatio);
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