namespace MPK.Connect.Service.Business.HarmonySearch
{
    public interface IHarmonyGenerator<T>
    {
        double HarmonyMemoryConsiderationRatio { get; set; }

        double PitchAdjustmentRatio { get; set; }

        ArgumentGenerationRules EstablishArgumentGenerationRule(double probability);

        T[] GenerateRandomArguments();

        Harmony<T> GenerateRandomHarmony();

        Harmony<T> GetHarmony(params T[] arguments);

        T[] ImproviseArguments();

        Harmony<T> ImproviseHarmony();

        T UseMemoryConsideration(int argumentIndex);

        T UsePitchAdjustment(int argumentIndex);

        T UseRandomChoosing(int argumentIndex);
    }
}