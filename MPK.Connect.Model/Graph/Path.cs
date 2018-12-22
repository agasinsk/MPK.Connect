using System;
using System.Collections.Generic;

namespace MPK.Connect.Model.Graph
{
    public class Path<T> : List<T> where T : class
    {
        public double Cost { get; set; }
        public DateTime StartDate { get; set; }

        public Path(IEnumerable<T> values, double cost) : base(values)
        {
            Cost = cost;
        }

        public Path()
        {
        }
    }
}