using System.Collections.Generic;

namespace MPK.Connect.Service.Utils
{
    public class MappingsDictionary : Dictionary<string, int>
    {
        public new int this[string key]
        {
            get => ContainsKey(key) ? base[key] : -1;
            set => base[key] = value;
        }

        public new void Add(string key, int value)
        {
            base.Add(key.Trim(), value);
        }
    }
}