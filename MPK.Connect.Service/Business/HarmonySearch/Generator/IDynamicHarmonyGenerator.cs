namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    public interface IDynamicHarmonyGenerator<T> : IHarmonyGenerator<T>
    {
        void MarkCurrentParametersAsWinning();
    }
}