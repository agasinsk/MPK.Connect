using System;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;

namespace MPK.Connect.Service.Business.HarmonySearch
{
    /// <summary>
    /// Generates harmonies
    /// </summary>
    public class HarmonyGenerator<T>
    {
        private readonly IRandomGenerator<T> _random;
        public double HarmonyMemoryConsiderationRatio { get; }
        public double PitchAdjustmentRatio { get; set; }
        private int ArgumentsCount => Function.GetArgumentsCount();
        private IObjectiveFunction<T> Function { get; }
        private HarmonyMemory<T> HarmonyMemory { get; }

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="function">Objective function</param>
        /// <param name="harmonyMemory">Harmony memory</param>
        /// <param name="harmonyMemoryConsiderationRatio">Harmony Memory Consideration Ratio</param>
        /// <param name="pitchAdjustmentRatio">Pitch Adjustment Ratio</param>
        /// <param name="randomGenerator">Random generator</param>
        public HarmonyGenerator(IObjectiveFunction<T> function,
            IRandomGenerator<T> randomGenerator,
            HarmonyMemory<T> harmonyMemory,
            double harmonyMemoryConsiderationRatio,
            double pitchAdjustmentRatio)
        {
            HarmonyMemoryConsiderationRatio = harmonyMemoryConsiderationRatio;
            PitchAdjustmentRatio = pitchAdjustmentRatio;

            _random = randomGenerator ?? throw new ArgumentNullException(nameof(randomGenerator));
            Function = function ?? throw new ArgumentNullException(nameof(function));
            HarmonyMemory = harmonyMemory ?? throw new ArgumentNullException(nameof(harmonyMemory));
        }

        /// <summary>
        /// Generates solution from random arguments
        /// </summary>
        public Harmony<T> GenerateRandomSolution()
        {
            var arguments = GenerateRandomArguments();
            return CalculateSolution(arguments);
        }

        /// <summary>
        /// Improvises new solution based on algorithm parameters
        /// </summary>
        public Harmony<T> ImproviseHarmony()
        {
            var improvisedArguments = ImproviseArguments();
            return CalculateSolution(improvisedArguments);
        }

        /// <summary>
        /// Calculates solution for provided arguments
        /// </summary>
        private Harmony<T> CalculateSolution(params T[] arguments)
        {
            var functionValue = Function.CalculateObjectiveValue();
            return new Harmony<T>(functionValue, arguments);
        }

        /// <summary>
        /// Chooses argument generation rule by their probabilities
        /// </summary>
        private ArgumentGenerationRules EstablishArgumentGenerationRule(double probability)
        {
            if (probability > HarmonyMemoryConsiderationRatio)
            {
                return ArgumentGenerationRules.RandomChoosing;
            }
            if (probability <= HarmonyMemoryConsiderationRatio * PitchAdjustmentRatio)
            {
                return ArgumentGenerationRules.PitchAdjustment;
            }
            return ArgumentGenerationRules.MemoryConsideration;
        }

        /// <summary>
        /// Generates random argument values
        /// </summary>
        private T[] GenerateRandomArguments()
        {
            var randomArguments = new T[ArgumentsCount];

            for (var index = 0; index < ArgumentsCount; index++)
            {
                randomArguments[index] = UseRandomChoosing(index);
            }

            return randomArguments;
        }

        /// <summary>
        /// Generates argument value based on the algorithm parameters
        /// </summary>
        /// <param name="argumentIndex">Index of argument</param>
        /// <returns>Generated argument</returns>
        private T ImproviseArgument(int argumentIndex)
        {
            var randomValue = _random.NextDouble();
            var generationRule = EstablishArgumentGenerationRule(randomValue);

            switch (generationRule)
            {
                case ArgumentGenerationRules.MemoryConsideration:
                    return UseMemoryConsideration(argumentIndex);

                case ArgumentGenerationRules.PitchAdjustment:
                    return UsePitchAdjustment(argumentIndex);

                case ArgumentGenerationRules.RandomChoosing:
                    return UseRandomChoosing(argumentIndex);

                default:
                    return default(T);
            }
        }

        /// <summary>
        /// Generates arguments values based on the algorithm parameters
        /// </summary>
        private T[] ImproviseArguments()
        {
            var arguments = new T[ArgumentsCount];

            for (var index = 0; index < ArgumentsCount; index++)
            {
                arguments[index] = ImproviseArgument(index);
            }

            return arguments;
        }

        /// <summary>
        /// Adjusts pitch for continuous variables
        /// </summary>
        private T UseContinuousPitchAdjustment(int argumentIndex)
        {
            var existingValue = UseMemoryConsideration(argumentIndex);
            var randomValue = _random.NextDouble();

            if (randomValue < 0.5)
            {
                var pitchAdjustment = _random.Next(Function.GetLowerBound(argumentIndex), existingValue);
                return Function.Subtract(existingValue, pitchAdjustment);
            }
            else
            {
                var pitchAdjustment = _random.Next(existingValue, Function.GetUpperBound(argumentIndex));
                return Function.Add(existingValue, pitchAdjustment);
            }
        }

        /// <summary>
        /// Adjusts pitch for discrete variables
        /// </summary>
        private T UseDiscretePitchAdjustment(int argumentIndex)
        {
            var existingValue = UseMemoryConsideration(argumentIndex);
            var currentValueIndex = Function.GetIndexOfDiscreteValue(argumentIndex, existingValue);
            var randomValue = _random.NextDouble();

            var maximumPitchAdjustmentIndex = Function.GetMaximumDiscretePitchAdjustmentIndex();
            if (randomValue < 0.5)
            {
                var allowedDiscreteValueIndex = Math.Min(maximumPitchAdjustmentIndex, currentValueIndex);
                var nextValueIndex = currentValueIndex - _random.Next(0, allowedDiscreteValueIndex);
                return Function.GetArgumentValue(argumentIndex, nextValueIndex);
            }
            else
            {
                var allowedDiscreteValueIndex = Math.Min(maximumPitchAdjustmentIndex, Function.GetPossibleDiscreteValuesCount(argumentIndex) - currentValueIndex - 1);
                var nextDiscreteValueIndex = currentValueIndex + _random.Next(0,
                                                 allowedDiscreteValueIndex);
                return Function.GetArgumentValue(argumentIndex, nextDiscreteValueIndex);
            }
        }

        /// <summary>
        /// Uses memory consideration technique
        /// </summary>
        /// <param name="argumentIndex">Index of argument</param>
        /// <returns></returns>
        private T UseMemoryConsideration(int argumentIndex)
        {
            return HarmonyMemory.GetRandomArgumentByIndex(argumentIndex);
        }

        /// <summary>
        /// Uses pitch adjusting technique
        /// </summary>
        /// <param name="argumentIndex">Index of argument</param>
        /// <returns>Pitch adjusted argument</returns>
        private T UsePitchAdjustment(int argumentIndex)
        {
            if (Function.IsArgumentVariable(argumentIndex))
            {
                if (Function.IsArgumentDiscrete(argumentIndex))
                {
                    return UseDiscretePitchAdjustment(argumentIndex);
                }

                return UseContinuousPitchAdjustment(argumentIndex);
            }

            return default(T);
        }

        /// <summary>
        /// Uses random choosing technique
        /// </summary>
        /// <param name="argumentIndex">Index of argument</param>
        /// <returns>Randomly chosen argument within defined bounds</returns>
        private T UseRandomChoosing(int argumentIndex)
        {
            return Function.GetArgumentValue(argumentIndex);
        }
    }
}