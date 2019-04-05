using MPK.Connect.Service.Business.HarmonySearch.Core;

namespace MPK.Connect.Service.Business.HarmonySearch.ParameterProviders
{
    /// <summary>
    /// Constant parameter provider
    /// </summary>
    public class ConstantParameterProvider : IParameterProvider
    {
        public double HarmonyMemoryConsiderationRatio { get; }
        public HarmonySearchType HarmonySearchType => HarmonySearchType.Standard;

        public double PitchAdjustmentRatio { get; }

        public ConstantParameterProvider(double harmonyMemoryConsiderationRatio, double pitchAdjustmentRatio)
        {
            HarmonyMemoryConsiderationRatio = harmonyMemoryConsiderationRatio;
            PitchAdjustmentRatio = pitchAdjustmentRatio;
        }
    }
}