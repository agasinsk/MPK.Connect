using MPK.Connect.Model;
using System.Collections.Generic;

namespace MPK.Connect.Service.Helpers
{
    public class ShapeBaseComparer : IEqualityComparer<ShapeBase>
    {
        public bool Equals(ShapeBase x, ShapeBase y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(ShapeBase obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}