﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace MPK.Connect.Model.Helpers
{
    public abstract class IdentifiableEntity<T>
    {
        public virtual T Id { get; set; }

        public List<PropertyInfo> GetRequiredProperties()
        {
            return GetType()
                .GetProperties()
                .Where(prop => prop.IsDefined(typeof(RequiredAttribute), false))
                .ToList();
        }
    }
}