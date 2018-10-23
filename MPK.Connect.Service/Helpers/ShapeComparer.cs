using MPK.Connect.Model;
using System.Collections.Generic;

namespace MPK.Connect.Service.Helpers
{
    public class ShapeComparer : IEqualityComparer<Shape>
    {
        public bool Equals(Shape x, Shape y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(Shape obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}