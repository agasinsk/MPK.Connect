using System.Collections.Generic;
using MPK.Connect.Model.Helpers;

namespace MPK.Connect.Service.Utils
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