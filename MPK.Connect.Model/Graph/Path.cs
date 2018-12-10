using System.Collections.Generic;

namespace MPK.Connect.Model.Graph
{
    public class Path<T> : List<T> where T : class
    {
        public double Cost { get; set; }
    }
}