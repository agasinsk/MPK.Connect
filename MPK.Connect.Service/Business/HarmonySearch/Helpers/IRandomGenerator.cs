namespace MPK.Connect.Service.Business.HarmonySearch.Helpers
{
    public interface IRandomGenerator<T>
    {
        int Next(int minValue, int maxValue);

        T Next(T minValue, T maxValue);

        double NextDouble();
    }
}