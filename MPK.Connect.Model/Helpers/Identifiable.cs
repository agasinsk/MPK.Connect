using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace MPK.Connect.Model.Helpers
{
    public abstract class Identifiable<T> : IIdentifiable
    {
        public virtual T Id { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Identifiable<T> identifiable &&
                   EqualityComparer<T>.Default.Equals(Id, identifiable.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public List<PropertyInfo> GetRequiredProperties()
        {
            return GetType()
                .GetProperties()
                .Where(prop => prop.IsDefined(typeof(RequiredAttribute), false))
                .ToList();
        }

        public bool HasDistinctId()
        {
            return GetType()
                .GetCustomAttributes(
                    typeof(DistinctIdAttribute), true
                ).FirstOrDefault() is DistinctIdAttribute;
        }
    }
}