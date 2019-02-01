using MPK.Connect.Service.Business.HarmonySearch.Constants;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;

namespace MPK.Connect.Service.Business.HarmonySearch
{
    /// <inheritdoc/>
    /// <summary>
    /// Implements harmony search algorithm
    /// </summary>
    public class HarmonySearcher<T>
    {
        private readonly HarmonyGenerator<T> _harmonyGenerator;
        public HarmonyMemory<T> HarmonyMemory { get; }
        public int ImprovisationCount { get; set; }
        public long MaxImprovisationCount { get; }
        public double HarmonyMemoryConsiderationRatio => _harmonyGenerator.HarmonyMemoryConsiderationRatio;
        public double PitchAdjustmentRatio => _harmonyGenerator.PitchAdjustmentRatio;

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="function">Function to optimize</param>
        /// <param name="randomGenerator">Random generator</param>
        public HarmonySearcher(IObjectiveFunction<T> function, IRandomGenerator<T> randomGenerator)
        {
            MaxImprovisationCount = HarmonySearchConstants.DefaultMaxImprovisationCount;
            HarmonyMemory = new HarmonyMemory<T>(HarmonySearchConstants.DefaultHarmonyMemorySize);
            _harmonyGenerator = new HarmonyGenerator<T>(function, randomGenerator, HarmonyMemory, HarmonySearchConstants.DefaultHarmonyMemoryConsiderationRatio, HarmonySearchConstants.DefaultPitchAdjustmentRatio);
        }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="function">Function to optimize</param>
        /// <param name="randomGenerator">Random generator</param>
        /// <param name="harmonyMemorySize">Harmony memory size</param>
        /// <param name="maxImprovisationCount">Maximum improvisation count</param>
        /// <param name="harmonyMemoryConsiderationRatio">Harmony Memory Consideration Ratio</param>
        /// <param name="pitchAdjustmentRatio">Pitch Adjustment Ratio</param>
        public HarmonySearcher(IObjectiveFunction<T> function, IRandomGenerator<T> randomGenerator, int harmonyMemorySize, long maxImprovisationCount, double harmonyMemoryConsiderationRatio, double pitchAdjustmentRatio)
        {
            MaxImprovisationCount = maxImprovisationCount;

            HarmonyMemory = new HarmonyMemory<T>(harmonyMemorySize);
            _harmonyGenerator = new HarmonyGenerator<T>(function, randomGenerator, HarmonyMemory, harmonyMemoryConsiderationRatio, pitchAdjustmentRatio);
        }

        /// <summary>
        /// Initializes harmony memory with random solutions
        /// </summary>
        public void InitializeHarmonyMemory()
        {
            for (var i = 0; i < HarmonyMemory.MaxCapacity; i++)
            {
                var randomSolution = _harmonyGenerator.GenerateRandomSolution();
                HarmonyMemory.Add(randomSolution);
            }
        }

        /// <summary>
        /// Looks for optimal solution of a function
        /// </summary>
        public Harmony<T> SearchForHarmony()
        {
            InitializeHarmonyMemory();

            ImprovisationCount = 0;
            while (SearchingShouldContinue())
            {
                var worstHarmony = HarmonyMemory.WorstHarmony;
                var improvisedHarmony = _harmonyGenerator.ImproviseHarmony();

                if (improvisedHarmony.IsBetterThan(worstHarmony))
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
        private bool SearchingShouldContinue()
        {
            return ImprovisationCount < MaxImprovisationCount;
        }
    }
}