using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;

namespace MPK.Connect.Service.Helpers
{
    public static class CollectionExtensions
    {
        public static T GetRandomElement<T>(this ICollection<T> collection)
        {
            var randomIndex = RandomFactory.GetInstance().Next(0, collection.Count);

            return collection.ElementAtOrDefault(randomIndex);
        }

        public static KeyValuePair<int, T> GetRandomElementWithIndex<T>(this ICollection<T> collection)
        {
            var randomIndex = RandomFactory.GetInstance().Next(0, collection.Count);
            var randomElement = collection.ElementAtOrDefault(randomIndex);

            return new KeyValuePair<int, T>(randomIndex, randomElement);
        }
    }
}