using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace MPK.Connect.TestEnvironment.Helpers
{
    public static class EnumExtensions
    {
        /// <summary>
        /// A generic extension method that aids in reflecting and retrieving any attribute that is
        /// applied to an `Enum`.
        /// </summary>
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
            where TAttribute : Attribute
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<TAttribute>();
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            var displayAttribute = enumValue.GetAttribute<DisplayAttribute>();

            return displayAttribute != null ? displayAttribute.Name : enumValue.ToString();
        }
    }

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