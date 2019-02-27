using MPK.Connect.Model.Helpers;
using System.Collections.Generic;

namespace MPK.Connect.Service.Helpers
{
    public class IdentifiableEntityComparer<T> : IEqualityComparer<Identifiable<T>>
    {
        public bool Equals(Identifiable<T> x, Identifiable<T> y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(Identifiable<T> obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}