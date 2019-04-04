﻿using System;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;
using MPK.Connect.Service.Utils;

namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    public abstract class BaseHarmonyGenerator<T> : IHarmonyGenerator<T>
    {
        protected readonly IBoundedRandom Random;
        public HarmonyMemory<T> HarmonyMemory { get; set; }
        public double HarmonyMemoryConsiderationRatio { get; set; }
        public IObjectiveFunction<T> ObjectiveFunction { get; }
        public double PitchAdjustmentRatio { get; set; }

        protected BaseHarmonyGenerator(IObjectiveFunction<T> function,
            HarmonyMemory<T> harmonyMemory)
        {
            Random = RandomFactory.GetInstance();
            ObjectiveFunction = function ?? throw new ArgumentNullException(nameof(function));
            HarmonyMemory = harmonyMemory ?? throw new ArgumentNullException(nameof(harmonyMemory));
        }

        protected BaseHarmonyGenerator(IObjectiveFunction<T> function,
            HarmonyMemory<T> harmonyMemory,
            double harmonyMemoryConsiderationRatio,
            double pitchAdjustmentRatio) : this(function, harmonyMemory)
        {
            HarmonyMemoryConsiderationRatio = harmonyMemoryConsiderationRatio;
            PitchAdjustmentRatio = pitchAdjustmentRatio;
        }

        public virtual HarmonyGenerationRules EstablishHarmonyGenerationRule(double probability)
        {
            if (probability > HarmonyMemoryConsiderationRatio)
            {
                return HarmonyGenerationRules.RandomChoosing;
            }
            if (probability <= HarmonyMemoryConsiderationRatio * PitchAdjustmentRatio)
            {
                return HarmonyGenerationRules.PitchAdjustment;
            }

            return HarmonyGenerationRules.MemoryConsideration;
        }

        public abstract Harmony<T> GenerateRandomHarmony();

        public Harmony<T> GetHarmony(params T[] arguments)
        {
            var functionValue = ObjectiveFunction.CalculateObjectiveValue(arguments);

            return new Harmony<T>(functionValue, arguments);
        }

        public abstract Harmony<T> ImproviseHarmony();

        public virtual Harmony<T> UseMemoryConsideration()
        {
            return HarmonyMemory.GetRandomElement();
        }
    }
}