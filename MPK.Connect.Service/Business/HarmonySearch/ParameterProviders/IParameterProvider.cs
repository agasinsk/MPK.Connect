using MPK.Connect.Service.Business.HarmonySearch.Core;

namespace MPK.Connect.Service.Business.HarmonySearch.ParameterProviders
{
    /// <summary>
    /// The interface for dynamic parameter provider
    /// </summary>
    public interface IDynamicParameterProvider : IParameterProvider
    {
        /// <summary>
        /// Marks current parameters as winning
        /// </summary>
        void MarkCurrentParametersAsWinning();
    }

    /// <summary>
    /// The interface for parameter provider
    /// </summary>
    public interface IParameterProvider
    {
        /// <summary>
        /// Gets the HMCR
        /// </summary>
        double HarmonyMemoryConsiderationRatio { get; }

        /// <summary>
        /// Gets the harmony search type
        /// </summary>
        HarmonySearchType HarmonySearchType { get; }

        /// <summary>
        /// Gets the PAR
        /// </summary>
        double PitchAdjustmentRatio { get; }
    }
}