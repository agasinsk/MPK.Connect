namespace MPK.Connect.Service.Business.HarmonySearch.Generator
{
    public interface IContinuousHarmonyGenerator<T> : IArgumentHarmonyGenerator<T>
    {
        T GetLowerBound(int argumentIndex);

        T GetPitchDownAdjustedValue(int argumentIndex, T existingValue);

        T GetPitchUpAdjustedValue(int argumentIndex, T existingValue);

        T GetUpperBound(int argumentIndex);
    }
}