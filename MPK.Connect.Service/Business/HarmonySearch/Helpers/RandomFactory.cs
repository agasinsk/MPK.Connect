namespace MPK.Connect.Service.Business.HarmonySearch.Helpers
{
    public class RandomFactory
    {
        private static IBoundedRandom _instance = null;

        public static IBoundedRandom GetInstance()
        {
            return _instance ?? (_instance = new BoundedRandom());
        }
    }
}