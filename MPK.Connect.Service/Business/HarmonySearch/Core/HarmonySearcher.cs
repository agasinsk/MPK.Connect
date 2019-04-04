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
        public HarmonyGeneratorType HarmonyGeneratorType => HarmonyGenerator.Type;
        public HarmonyMemory<T> HarmonyMemory { get; }
        public int ImprovisationCount { get; set; }
        public long MaxImprovisationCount { get; set; }
        public double PitchAdjustmentRatio { get; set; }

        public virtual HarmonySearchType Type => HarmonySearchType.Standard;

        public double HarmonyMemoryConsiderationRatio => HarmonyGenerator.HarmonyMemoryConsiderationRatio;

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="harmonyGenerator">Harmony generator</param>
        public HarmonySearcher(IHarmonyGenerator<T> harmonyGenerator)
        {
            PitchAdjustmentRatio = DefaultPitchAdjustmentRatio;
            MaxImprovisationCount = DefaultMaxImprovisationCount;
            HarmonyGenerator = harmonyGenerator;

            HarmonyMemory = new HarmonyMemory<T>(DefaultHarmonyMemorySize);
            HarmonyGenerator.HarmonyMemory = HarmonyMemory;
        }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="harmonyGenerator">Harmony generator</param>
        /// <param name="harmonyMemorySize">Harmony memory size</param>
        public HarmonySearcher(IHarmonyGenerator<T> harmonyGenerator, int harmonyMemorySize) : this(harmonyGenerator)
        {
            HarmonyMemory = new HarmonyMemory<T>(harmonyMemorySize);
            HarmonyGenerator.HarmonyMemory = HarmonyMemory;
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