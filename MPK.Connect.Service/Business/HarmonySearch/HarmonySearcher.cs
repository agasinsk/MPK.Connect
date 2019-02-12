using MPK.Connect.Service.Business.HarmonySearch.Helpers;
using static MPK.Connect.Service.Business.HarmonySearch.Constants.HarmonySearchConstants;

namespace MPK.Connect.Service.Business.HarmonySearch
{
    /// <inheritdoc/>
    /// <summary>
    /// Implements harmony search algorithm
    /// </summary>
    public class HarmonySearcher<T>
    {
        private readonly IHarmonyGenerator<T> _harmonyGenerator;
        public HarmonyMemory<T> HarmonyMemory { get; }
        public int ImprovisationCount { get; set; }
        public long MaxImprovisationCount { get; }
        public double MaxPitchAdjustmentRatio { get; set; }
        public double MinPitchAdjustmentRatio { get; set; }
        public double PitchAdjustmentRatio { get; set; }
        public bool ShouldImprovePitchAdjustingScenario { get; }
        public double HarmonyMemoryConsiderationRatio => _harmonyGenerator.HarmonyMemoryConsiderationRatio;

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="function">Function to optimize</param>
        /// <param name="random">Random generator</param>
        public HarmonySearcher(IObjectiveFunction<T> function, IRandom random = null)
        {
            PitchAdjustmentRatio = DefaultPitchAdjustmentRatio;
            MaxImprovisationCount = DefaultMaxImprovisationCount;
            HarmonyMemory = new HarmonyMemory<T>(DefaultHarmonyMemorySize);

            _harmonyGenerator = HarmonyGeneratorFactory.GetHarmonyGenerator(function, HarmonyMemory,
                DefaultHarmonyMemoryConsiderationRatio, PitchAdjustmentRatio, random);
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

            _harmonyGenerator = HarmonyGeneratorFactory.GetHarmonyGenerator(function, HarmonyMemory, harmonyMemoryConsiderationRatio, PitchAdjustmentRatio);
        }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="function">Function to optimize</param>
        /// <param name="randomGenerator">Random generator</param>
        /// <param name="harmonyMemorySize">Harmony memory size</param>
        /// <param name="maxImprovisationCount">Maximum improvisation count</param>
        /// <param name="harmonyMemoryConsiderationRatio">Harmony Memory Consideration Ratio</param>
        /// <param name="minPitchAdjustmentRatio">Minimum Pitch Adjustment Ratio</param>
        /// <param name="maxPitchAdjustmentRatio">Maximum Pitch Adjustment Ratio</param>
        public HarmonySearcher(IObjectiveFunction<T> function, int harmonyMemorySize, long maxImprovisationCount, double harmonyMemoryConsiderationRatio, double minPitchAdjustmentRatio, double maxPitchAdjustmentRatio)
        {
            MaxImprovisationCount = maxImprovisationCount;
            HarmonyMemory = new HarmonyMemory<T>(harmonyMemorySize);

            ShouldImprovePitchAdjustingScenario = true;
            MinPitchAdjustmentRatio = minPitchAdjustmentRatio;
            MaxPitchAdjustmentRatio = maxPitchAdjustmentRatio;
            PitchAdjustmentRatio = GetCurrentPitchAdjustingRatio(0);

            _harmonyGenerator = HarmonyGeneratorFactory.GetHarmonyGenerator(function, HarmonyMemory, harmonyMemoryConsiderationRatio, PitchAdjustmentRatio);
        }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="function">Function to optimize</param>
        /// <param name="random">Random generator</param>
        /// <param name="harmonyMemorySize">Harmony memory size</param>
        /// <param name="maxImprovisationCount">Maximum improvisation count</param>
        /// <param name="harmonyMemoryConsiderationRatio">Harmony Memory Consideration Ratio</param>
        /// <param name="shouldImprovePitchAdjustingScenario"></param>
        public HarmonySearcher(IObjectiveFunction<T> function, int harmonyMemorySize, long maxImprovisationCount, double harmonyMemoryConsiderationRatio, bool shouldImprovePitchAdjustingScenario)
        {
            MaxImprovisationCount = maxImprovisationCount;
            HarmonyMemory = new HarmonyMemory<T>(harmonyMemorySize);

            ShouldImprovePitchAdjustingScenario = shouldImprovePitchAdjustingScenario;
            MinPitchAdjustmentRatio = DefaultMinPitchAdjustmentRatio;
            MaxPitchAdjustmentRatio = DefaultMaxPitchAdjustmentRatio;
            PitchAdjustmentRatio = GetCurrentPitchAdjustingRatio(0);

            _harmonyGenerator = HarmonyGeneratorFactory.GetHarmonyGenerator(function, HarmonyMemory, harmonyMemoryConsiderationRatio, PitchAdjustmentRatio);
        }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="function">Function to optimize</param>
        /// <param name="random">Random generator</param>
        /// <param name="shouldImprovePitchAdjustingScenario">
        /// Determines if pitch adjustment should be improved
        /// </param>
        public HarmonySearcher(IObjectiveFunction<T> function, bool shouldImprovePitchAdjustingScenario)
        {
            MaxImprovisationCount = DefaultMaxImprovisationCount;
            HarmonyMemory = new HarmonyMemory<T>(DefaultHarmonyMemorySize);

            ShouldImprovePitchAdjustingScenario = shouldImprovePitchAdjustingScenario;
            MinPitchAdjustmentRatio = DefaultMinPitchAdjustmentRatio;
            MaxPitchAdjustmentRatio = DefaultMaxPitchAdjustmentRatio;
            PitchAdjustmentRatio = GetCurrentPitchAdjustingRatio(0);

            _harmonyGenerator = HarmonyGeneratorFactory.GetHarmonyGenerator(function, HarmonyMemory, DefaultHarmonyMemoryConsiderationRatio, PitchAdjustmentRatio);
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

        /// <summary>
        /// Initializes harmony memory with random solutions
        /// </summary>
        public void InitializeHarmonyMemory()
        {
            for (var i = 0; i < HarmonyMemory.MaxCapacity; i++)
            {
                var randomSolution = _harmonyGenerator.GenerateRandomHarmony();
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
                if (ShouldImprovePitchAdjustingScenario)
                {
                    _harmonyGenerator.PitchAdjustmentRatio = GetCurrentPitchAdjustingRatio(ImprovisationCount);
                }

                var improvisedHarmony = _harmonyGenerator.ImproviseHarmony();
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
        private bool SearchingShouldContinue()
        {
            return ImprovisationCount < MaxImprovisationCount;
        }
    }
}