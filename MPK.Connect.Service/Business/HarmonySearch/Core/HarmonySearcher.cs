using System;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Generator;
using MPK.Connect.Service.Business.HarmonySearch.ParameterProviders;
using static MPK.Connect.Service.Business.HarmonySearch.Constants.HarmonySearchConstants;

namespace MPK.Connect.Service.Business.HarmonySearch.Core
{
    /// <inheritdoc/>
    /// <summary>
    /// Implements harmony search algorithm
    /// </summary>
    public class HarmonySearcher<T> : IHarmonySearcher<T>
    {
        protected readonly IHarmonyGenerator<T> HarmonyGenerator;
        protected readonly int MaxImprovisationCountWithTheSameBestValue;
        protected double BestHarmonyObjectiveValue;
        protected int ImprovisationCountWithTheSameBestValue;
        public HarmonyGeneratorType HarmonyGeneratorType => HarmonyGenerator.Type;
        public HarmonyMemory<T> HarmonyMemory { get; }
        public int ImprovisationCount { get; protected set; }
        public long MaxImprovisationCount { get; set; }
        public IParameterProvider ParameterProvider { get; }
        public virtual HarmonySearchType Type => ParameterProvider.HarmonySearchType;
        public ObjectiveFunctionType ObjectiveFunctionType => HarmonyGenerator.ObjectiveFunctionType;

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="harmonyGenerator">Harmony generator</param>
        /// <param name="parameterProvider"></param>
        /// <param name="harmonyMemorySize">Harmony memory size</param>
        /// <param name="maxImprovisationCount">Maximal improvisation count</param>
        public HarmonySearcher(IHarmonyGenerator<T> harmonyGenerator, IParameterProvider parameterProvider, int harmonyMemorySize = DefaultHarmonyMemorySize, long maxImprovisationCount = DefaultMaxImprovisationCount)
        {
            MaxImprovisationCount = maxImprovisationCount;

            HarmonyGenerator = harmonyGenerator ?? throw new ArgumentNullException(nameof(harmonyGenerator));
            ParameterProvider = parameterProvider ?? throw new ArgumentNullException(nameof(parameterProvider));

            HarmonyMemory = new HarmonyMemory<T>(harmonyMemorySize);
            HarmonyGenerator.HarmonyMemory = HarmonyMemory;

            BestHarmonyObjectiveValue = double.PositiveInfinity;
            MaxImprovisationCountWithTheSameBestValue = (int)(maxImprovisationCount / 10);
        }

        /// <summary>
        /// Initializes harmony memory with random solutions
        /// </summary>
        public virtual void InitializeHarmonyMemory()
        {
            for (var i = 0; i < HarmonyMemory.MaxCapacity; i++)
            {
                var generateRandomHarmony = HarmonyGenerator.GenerateRandomHarmony();

                HarmonyMemory.Add(generateRandomHarmony);
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
                var harmonyMemoryConsiderationRatio = ParameterProvider.HarmonyMemoryConsiderationRatio;
                var pitchAdjustmentRatio = ParameterProvider.PitchAdjustmentRatio;

                var improvisedHarmony = HarmonyGenerator.ImproviseHarmony(harmonyMemoryConsiderationRatio, pitchAdjustmentRatio);

                var worstHarmony = HarmonyMemory.WorstHarmony;

                if (improvisedHarmony.IsBetterThan(worstHarmony) && !HarmonyMemory.Contains(improvisedHarmony))
                {
                    HarmonyMemory.SwapWithWorstHarmony(improvisedHarmony);
                }

                SaveBestHarmony(HarmonyMemory.BestHarmony);

                ImprovisationCount++;
            }

            return HarmonyMemory.BestHarmony;
        }

        /// <summary>
        /// Saves the objective value of the best harmony
        /// </summary>
        /// <param name="bestHarmony">Best harmony in current iteration</param>
        protected virtual void SaveBestHarmony(Harmony<T> bestHarmony)
        {
            if (Math.Abs(BestHarmonyObjectiveValue - bestHarmony.ObjectiveValue) < 0.001)
            {
                ImprovisationCountWithTheSameBestValue++;
            }

            BestHarmonyObjectiveValue = bestHarmony.ObjectiveValue;
        }

        /// <summary>
        /// Checks if algorithm should continue working
        /// </summary>
        protected virtual bool SearchingShouldContinue()
        {
            return ImprovisationCount < MaxImprovisationCount && ImprovisationCountWithTheSameBestValue < MaxImprovisationCountWithTheSameBestValue;
        }
    }
}