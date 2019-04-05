using MPK.Connect.Service.Business.HarmonySearch.Generator;
using MPK.Connect.Service.Business.HarmonySearch.ParameterProviders;

namespace MPK.Connect.Service.Business.HarmonySearch.Core
{
    public class HarmonySearcherFactory
    {
        public static IHarmonySearcher<T> GetInstance<T>(HarmonySearchType type, IHarmonyGenerator<T> generator, int harmonyMemorySize, long maxImprovisationCount, double harmonyMemoryConsiderationRatio, double pitchAdjustmentRatio, double minPitchAdjustmentRatio, double maxPitchAdjustmentRatio)
        {
            switch (type)
            {
                default:
                    {
                        var parameterProvider = new ConstantParameterProvider(harmonyMemoryConsiderationRatio, pitchAdjustmentRatio);

                        return new HarmonySearcher<T>(generator, parameterProvider, harmonyMemorySize,
                            maxImprovisationCount);
                    }

                case HarmonySearchType.Improved:
                    {
                        var parameterProvider = new DynamicPitchAdjustmentRatioProvider(harmonyMemoryConsiderationRatio, maxPitchAdjustmentRatio, minPitchAdjustmentRatio, maxImprovisationCount);
                        return new HarmonySearcher<T>(generator, parameterProvider, harmonyMemorySize,
                            maxImprovisationCount);
                    }
                case HarmonySearchType.Dynamic:
                    {
                        var parameterProvider = new DynamicParameterProvider(harmonyMemorySize * 10);
                        return new HarmonySearcher<T>(generator, parameterProvider, harmonyMemorySize,
                            maxImprovisationCount);
                    }
                case HarmonySearchType.Divided:
                    {
                        var parameterProvider = new ConstantParameterProvider(harmonyMemoryConsiderationRatio, pitchAdjustmentRatio);

                        return new DividedHarmonySearcher<T>(generator, parameterProvider, harmonyMemorySize,
                            maxImprovisationCount);
                    }
                case HarmonySearchType.ImprovedDivided:
                    {
                        var parameterProvider = new DynamicPitchAdjustmentRatioProvider(harmonyMemoryConsiderationRatio, maxPitchAdjustmentRatio, minPitchAdjustmentRatio, maxImprovisationCount);

                        return new DividedHarmonySearcher<T>(generator, parameterProvider, harmonyMemorySize,
                        maxImprovisationCount);
                    }
                case HarmonySearchType.DynamicDivided:
                    {
                        var parameterProvider = new DynamicParameterProvider(harmonyMemorySize * 10);

                        return new DividedHarmonySearcher<T>(generator, parameterProvider, harmonyMemorySize,
                            maxImprovisationCount);
                    }
            }
        }
    }
}