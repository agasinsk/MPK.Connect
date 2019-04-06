using System;
using System.Reflection;

namespace MPK.Connect.Service.Utils
{
    public static class ObjectExtensions
    {
        public static T GetPropValue<T>(this Object obj, string name)
        {
            var propValue = GetPropValue(obj, name);
            if (propValue == null)
            {
                return default;
            }

            // throws InvalidCastException if types are incompatible
            return (T)propValue;
        }

        public static Object GetPropValue(this Object obj, string name)
        {
            foreach (string part in name.Split('.'))
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }
    }
}