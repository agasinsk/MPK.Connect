using System;
using System.Collections.Generic;

namespace MPK.Connect.Service.Business.HarmonySearch.Helpers
{
    public class DuplicateComparer<TKey> : IComparer<TKey> where TKey : IComparable
    {
        #region IComparer<TKey> Members

        public int Compare(TKey x, TKey y)
        {
            int result = x.CompareTo(y);

            if (result == 0)
                return -1;   // Handle equality as being lower

            return result;
        }

        #endregion IComparer<TKey> Members
    }
}