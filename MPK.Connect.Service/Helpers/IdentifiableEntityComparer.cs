using MPK.Connect.Model.Helpers;
using System.Collections.Generic;

namespace MPK.Connect.Service.Helpers
{
    public class IdentifiableEntityComparer<T> : IEqualityComparer<IdentifiableEntity<T>>
    {
        public bool Equals(IdentifiableEntity<T> x, IdentifiableEntity<T> y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(IdentifiableEntity<T> obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}