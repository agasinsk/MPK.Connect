using System;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;

namespace MPK.Connect.Service.Business.HarmonySearch
{
    public abstract class HarmonyGeneratorBase<T> : IHarmonyGenerator<T>
    {
        public HarmonyMemory<T> HarmonyMemory;
        protected readonly IRandom Random;
        protected IObjectiveFunction<T> Function;
        public double HarmonyMemoryConsiderationRatio { get; set; }
        public double PitchAdjustmentRatio { get; set; }
        protected int ArgumentsCount => Function.GetArgumentsCount();

        protected HarmonyGeneratorBase(IObjectiveFunction<T> function,
            HarmonyMemory<T> harmonyMemory,
            double harmonyMemoryConsiderationRatio,
            double pitchAdjustmentRatio)
        {
            HarmonyMemoryConsiderationRatio = harmonyMemoryConsiderationRatio;
            PitchAdjustmentRatio = pitchAdjustmentRatio;

            Random = RandomFactory.GetInstance();
            Function = function ?? throw new ArgumentNullException(nameof(function));
            HarmonyMemory = harmonyMemory ?? throw new ArgumentNullException(nameof(harmonyMemory));
        }

        public ArgumentGenerationRules EstablishArgumentGenerationRule(double probability)
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

        public T[] GenerateRandomArguments()
        {
            var randomArguments = new T[ArgumentsCount];

            for (var index = 0; index < ArgumentsCount; index++)
            {
                randomArguments[index] = UseRandomChoosing(index);
            }

            return randomArguments;
        }

        public Harmony<T> GenerateRandomHarmony()
        {
            var arguments = GenerateRandomArguments();
            return GetHarmony(arguments);
        }

        public Harmony<T> GetHarmony(params T[] arguments)
        {
            var functionValue = Function.CalculateObjectiveValue(arguments);
            return new Harmony<T>(functionValue, arguments);
        }

        public T[] ImproviseArguments()
        {
            var arguments = new T[ArgumentsCount];

            for (var index = 0; index < ArgumentsCount; index++)
            {
                arguments[index] = ImproviseArgument(index);
            }

            return arguments;
        }

        public Harmony<T> ImproviseHarmony()
        {
            var improvisedArguments = ImproviseArguments();
            return GetHarmony(improvisedArguments);
        }

        public virtual T UseMemoryConsideration(int argumentIndex)
        {
            return HarmonyMemory.GetArgumentFromRandomHarmony(argumentIndex);
        }

        public abstract T UsePitchAdjustment(int argumentIndex);

        public virtual T UseRandomChoosing(int argumentIndex)
        {
            return Function.GetArgumentValue(argumentIndex);
        }

        /// <summary>
        /// Generates argument value based on the algorithm parameters
        /// </summary>
        /// <param name="argumentIndex">Index of argument</param>
        /// <returns>Generated argument</returns>
        protected T ImproviseArgument(int argumentIndex)
        {
            var randomValue = Random.NextDouble();
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
    }
}