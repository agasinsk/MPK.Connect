using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;

namespace MPK.Connect.Service.Helpers
{
    public static class CollectionExtensions
    {
        public static T GetRandomElement<T>(this ICollection<T> collection)
        {
            var randomIndex = RandomFactory.GetInstance().Next(collection.Count);

            return collection.ElementAtOrDefault(randomIndex);
        }

        public static KeyValuePair<int, T> GetRandomElementWithIndex<T>(this ICollection<T> collection)
        {
            var randomIndex = RandomFactory.GetInstance().Next(collection.Count);
            var randomElement = collection.ElementAtOrDefault(randomIndex);

            return new KeyValuePair<int, T>(randomIndex, randomElement);
        }

        public static int GetRandomIndex<T>(this ICollection<T> collection)
        {
            return RandomFactory.GetInstance().Next(collection.Count);
        }

        public static int GetRandomIndexMinimum<T>(this ICollection<T> collection, int startIndex)
        {
            return RandomFactory.GetInstance().Next(startIndex, collection.Count);
        }
    }
}