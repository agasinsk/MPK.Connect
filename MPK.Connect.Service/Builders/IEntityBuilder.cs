using System.Collections.Generic;

namespace MPK.Connect.Service.Builders
{
    public interface IEntityBuilder<T> where T : class
    {
        IDictionary<string, int> GetEntityMappings(string headerString);

        T Build(string dataString, IDictionary<string, int> mappings);
    }
}