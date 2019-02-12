﻿namespace MPK.Connect.Service.Business.HarmonySearch.Helpers
{
    public class RandomFactory
    {
        private static IBoundedRandom _random;

        public static IBoundedRandom GetInstance()
        {
            return _random ?? (_random = new BoundedRandom());
        }
    }
}