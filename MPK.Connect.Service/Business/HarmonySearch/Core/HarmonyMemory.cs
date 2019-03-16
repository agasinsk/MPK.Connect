using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;
using MPK.Connect.Service.Helpers;
using MPK.Connect.Service.Utils;

namespace MPK.Connect.Service.Business.HarmonySearch.Core
{
    /// <inheritdoc/>
    /// <summary>
    /// Stores best solutions for harmony search algorithm
    /// </summary>
    public class HarmonyMemory<T> : ICollection<Harmony<T>>
    {
        private readonly BoundedSortedSet<Harmony<T>> _harmonies;
        public Harmony<T> BestHarmony => _harmonies.First();
        public bool IsReadOnly => false;
        public int MaxCapacity => _harmonies.Capacity;
        public Harmony<T> WorstHarmony => _harmonies.Last();
        public int Count => _harmonies.Count;

        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="harmonyMemorySize">Size of harmony memory</param>
        public HarmonyMemory(int harmonyMemorySize)
        {
            _harmonies = new BoundedSortedSet<Harmony<T>>(harmonyMemorySize, new HarmonyComparer<T>());
        }

        /// <summary>
        /// Adds new solution to harmony memory
        /// </summary>
        /// <param name="harmony">New solution</param>
        /// <returns>Result of addition</returns>
        public bool Add(Harmony<T> harmony)
        {
            return _harmonies.Add(harmony);
        }

        /// <summary>
        /// Adds new solutions to harmony memory
        /// </summary>
        /// <param name="harmonies">New harmonies</param>
        /// <returns>Result of additions</returns>
        public bool AddRange(params Harmony<T>[] harmonies)
        {
            return harmonies.All(harmony => _harmonies.Add(harmony));
        }

        /// <summary>
        /// Adds new solutions to harmony memory
        /// </summary>
        /// <param name="harmonies">New harmonies</param>
        /// <returns>Result of additions</returns>
        public bool AddRange(IEnumerable<Harmony<T>> harmonies)
        {
            return harmonies.All(harmony => _harmonies.Add(harmony));
        }

        /// <summary>
        /// Clears harmony memory
        /// </summary>
        public void Clear()
        {
            _harmonies.Clear();
        }

        /// <summary>
        /// Checks if memory contains certain harmony
        /// </summary>
        /// <param name="harmony">Harmony to check</param>
        /// <returns>True if harmony exists in memory</returns>
        public bool Contains(Harmony<T> harmony)
        {
            return _harmonies.Contains(harmony);
        }

        public void CopyTo(Harmony<T>[] array, int arrayIndex)
        {
            _harmonies.CopyTo(array, arrayIndex);
        }

        public IEnumerable<Harmony<T>> GetAll()
        {
            return _harmonies.AsEnumerable();
        }

        /// <summary>
        /// Gets random argument
        /// </summary>
        /// <param name="argumentIndex">Index of argument</param>
        /// <returns></returns>
        public T GetArgumentFromRandomHarmony(int argumentIndex)
        {
            return _harmonies.GetRandomElement().GetArgument(argumentIndex);
        }

        /// <summary>
        /// Gets list of arguments by their index
        /// </summary>
        /// <param name="argumentIndex">Index of arguments</param>
        /// <returns>List of arguments</returns>
        public List<T> GetArguments(int argumentIndex)
        {
            return _harmonies.Select(s => s.GetArgument(argumentIndex)).ToList();
        }

        /// <inheritdoc/>
        public IEnumerator<Harmony<T>> GetEnumerator()
        {
            return _harmonies.GetEnumerator();
        }

        public bool Remove(Harmony<T> harmony)
        {
            return _harmonies.Remove(harmony);
        }

        /// <summary>
        /// Swaps new harmony with the current worst harmony
        /// </summary>
        /// <param name="harmony">Harmony to be placed into memory</param>
        public void SwapWithWorstHarmony(Harmony<T> harmony)
        {
            _harmonies.Remove(WorstHarmony);
            _harmonies.Add(harmony);
        }

        void ICollection<Harmony<T>>.Add(Harmony<T> harmony)
        {
            Add(harmony);
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _harmonies.GetEnumerator();
        }
    }
}