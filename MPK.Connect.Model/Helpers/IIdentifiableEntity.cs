using System.Collections.Generic;
using System.Reflection;

namespace MPK.Connect.Model.Helpers
{
    public interface IIdentifiableEntity
    {
        List<PropertyInfo> GetRequiredProperties();
        bool HasDistinctId();
    }
}