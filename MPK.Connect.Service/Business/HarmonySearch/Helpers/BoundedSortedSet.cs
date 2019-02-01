﻿using System.Collections.Generic;

namespace MPK.Connect.Service.Business.HarmonySearch.Helpers
{
    /// <inheritdoc />
    /// <summary>
    /// Sorted set with limited capacity
    /// </summary>
    /// <typeparam name="T">Stored elements</typeparam>
    public class BoundedSortedSet<T> : SortedSet<T>
    {
        public int Capacity { get; }

        /// <inheritdoc />
        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="capacity">Maximum capacity of set</param>
        public BoundedSortedSet(int capacity)
        {
            Capacity = capacity;
        }

        /// <summary>
        /// Adds new element if set is not already full
        /// </summary>
        /// <param name="element">Element to add</param>
        /// <returns>True if added</returns>
        public new bool Add(T element)
        {
            return Count < Capacity && base.Add(element);
        }
    }
}