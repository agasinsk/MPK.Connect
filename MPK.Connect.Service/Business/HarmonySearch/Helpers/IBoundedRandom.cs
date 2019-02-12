namespace MPK.Connect.Service.Business.HarmonySearch.Helpers
{
    public interface IBoundedRandom : IRandom
    {
        double NextValue(double minValue, double maxValue);
    }
}