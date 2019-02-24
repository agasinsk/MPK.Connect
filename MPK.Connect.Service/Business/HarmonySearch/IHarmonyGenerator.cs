namespace MPK.Connect.Service.Business.HarmonySearch
{
    public interface IHarmonyGenerator<T>
    {
        double HarmonyMemoryConsiderationRatio { get; set; }

        double PitchAdjustmentRatio { get; set; }

        Harmony<T> GenerateRandomHarmony();

        Harmony<T> ImproviseHarmony();
    }
}