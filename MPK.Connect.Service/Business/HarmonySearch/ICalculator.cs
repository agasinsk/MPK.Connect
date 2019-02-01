namespace MPK.Connect.Service.Business.HarmonySearch
{
    public interface ICalculator<T>
    {
        T Add(T firstValue, T secondValue);

        T Divide(T firstValue, T secondValue);

        T Multiply(T firstValue, T secondValue);

        T Subtract(T firstValue, T secondValue);
    }
}