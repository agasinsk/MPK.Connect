namespace MPK.Connect.Service.Business.HarmonySearch.Functions
{
    public interface IContinuousObjectiveFunction<T> : IArgumentObjectiveFunction<T>
    {
        T GetLowerBound(int argumentIndex);

        T GetPitchDownAdjustedValue(int argumentIndex, T existingValue);

        T GetPitchUpAdjustedValue(int argumentIndex, T existingValue);

        T GetUpperBound(int argumentIndex);
    }
}