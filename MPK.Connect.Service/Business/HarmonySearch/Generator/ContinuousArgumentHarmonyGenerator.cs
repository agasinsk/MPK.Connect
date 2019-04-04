//using System;
//using MPK.Connect.Service.Business.HarmonySearch.Core;
//using MPK.Connect.Service.Business.HarmonySearch.Functions;

//namespace MPK.Connect.Service.Business.HarmonySearch.Generator
//{
//    public class ContinuousArgumentHarmonyGenerator<T> : ArgumentHarmonyGenerator<T>
//    {
//        protected new IContinuousObjectiveFunction<T> ObjectiveFunction;

// public ContinuousArgumentHarmonyGenerator(IContinuousObjectiveFunction<T> function,
// HarmonyMemory<T> harmonyMemory, double harmonyMemoryConsiderationRatio, double
// pitchAdjustmentRatio) : base(function, harmonyMemory, harmonyMemoryConsiderationRatio,
// pitchAdjustmentRatio) { ObjectiveFunction = function ?? throw new
// ArgumentNullException(nameof(function)); }

// public override T UsePitchAdjustment(int argumentIndex) { var existingValue =
// UseMemoryConsideration(argumentIndex); var randomValue = Random.NextDouble();

// if (randomValue < 0.5) { return ObjectiveFunction.GetPitchDownAdjustedValue(argumentIndex,
// existingValue); }

//            return ObjectiveFunction.GetPitchUpAdjustedValue(argumentIndex, existingValue);
//        }
//    }
//}