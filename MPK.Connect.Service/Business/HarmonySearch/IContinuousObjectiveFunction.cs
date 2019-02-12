namespace MPK.Connect.Service.Business.HarmonySearch
{
    public interface IContinuousObjectiveFunction<T> : IObjectiveFunction<T>
    {
        T GetLowerBound(int argumentIndex);

        T GetPitchDownAdjustedValue(int argumentIndex, T existingValue);

        T GetPitchUpAdjustedValue(int argumentIndex, T existingValue);

        T GetUpperBound(int argumentIndex);
    }
}