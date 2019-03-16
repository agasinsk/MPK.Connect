using System;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;

namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    public abstract class ArgumentHarmonyGenerator<T> : HarmonyGeneratorBase<T>
    {
        protected new IArgumentObjectiveFunction<T> ObjectiveFunction;
        protected int ArgumentsCount => ObjectiveFunction.GetArgumentsCount();

        protected ArgumentHarmonyGenerator(IArgumentObjectiveFunction<T> function, HarmonyMemory<T> harmonyMemory, double harmonyMemoryConsiderationRatio, double pitchAdjustmentRatio) : base(function, harmonyMemory, harmonyMemoryConsiderationRatio, pitchAdjustmentRatio)
        {
            ObjectiveFunction = function ?? throw new ArgumentNullException(nameof(function));
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

        public override Harmony<T> GenerateRandomHarmony()
        {
            var arguments = GenerateRandomArguments();
            return GetHarmony(arguments);
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

        public override Harmony<T> ImproviseHarmony()
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
            return ObjectiveFunction.GetArgumentValue(argumentIndex);
        }

        /// <summary>
        /// Generates argument value based on the algorithm parameters
        /// </summary>
        /// <param name="argumentIndex">Index of argument</param>
        /// <returns>Generated argument</returns>
        protected T ImproviseArgument(int argumentIndex)
        {
            var randomValue = Random.NextDouble();
            var generationRule = EstablishHarmonyGenerationRule(randomValue);

            switch (generationRule)
            {
                case HarmonyGenerationRules.MemoryConsideration:
                    return UseMemoryConsideration(argumentIndex);

                case HarmonyGenerationRules.PitchAdjustment:
                    return UsePitchAdjustment(argumentIndex);

                case HarmonyGenerationRules.RandomChoosing:
                    return UseRandomChoosing(argumentIndex);

                default:
                    return default(T);
            }
        }
    }
}