using System.Collections.Generic;
using MPK.Connect.Model;

namespace MPK.Connect.Service.Utils
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