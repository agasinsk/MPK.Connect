using System.Collections.Generic;
using System.Reflection;

namespace MPK.Connect.Model.Helpers
{
    public interface IIdentifiable
    {
        List<PropertyInfo> GetRequiredProperties();

        bool HasDistinctId();
    }
}