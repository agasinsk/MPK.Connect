namespace MPK.Connect.Service.Business.HarmonySearch.Helpers
{
    public interface IRandomGenerator<T>
    {
        int Next(int minValue, int maxValue);

        double NextDouble();

        T NextValue(T minValue, T maxValue);
    }
}