using System.Collections.Generic;

namespace MPK.Connect.Service.Builders
{
    public abstract class BaseEntityBuilder<T> : IEntityBuilder<T> where T : class
    {
        public abstract T Build(string dataString, IDictionary<string, int> mappings);

        public virtual IDictionary<string, int> GetEntityMappings(string headerString)
        {
            var mappings = new Dictionary<string, int>();

            var headers = headerString.Split(',');
            var headerIndex = 0;
            foreach (var header in headers)
            {
                mappings.Add(header, headerIndex);
                headerIndex++;
            }

            return mappings;
        }
    }
}