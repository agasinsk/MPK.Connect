using System;
using System.Collections.Generic;
using System.Linq;

namespace MPK.Connect.TestEnvironment.Helpers
{
    public static class EnumUtils<T> where T : Enum
    {
        public static IEnumerable<T> GetEnumValues()
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException($"Provided type {typeof(T).Name} is not an enum");
            }

            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }
    }
}