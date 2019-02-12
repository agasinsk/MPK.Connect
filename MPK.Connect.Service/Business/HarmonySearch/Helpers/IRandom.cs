namespace MPK.Connect.Service.Business.HarmonySearch.Helpers
{
    public interface IRandom
    {
        int Next(int minValue, int maxValue);

        int Next(int maxValue);

        double NextDouble();
    }
}