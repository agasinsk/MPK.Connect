using System;
using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;
using MPK.Connect.Service.Utils;

namespace MPK.Connect.Service.Business.HarmonySearch.ParameterProviders
{
    /// <inheritdoc/>
    /// <summary>
    /// Contains logic for providing dynamic parameter values (Winning Parameter Set List)
    /// </summary>
    public class DynamicParameterProvider : IParameterProvider
    {
        private readonly IBoundedRandom _random;
        private int _currentParameterSetIndex;
        public double HarmonyMemoryConsiderationRatio => GetCurrentHarmonyMemoryConsiderationRatio();

        public HarmonySearchType HarmonySearchType => HarmonySearchType.Dynamic;
        public List<Tuple<double, double>> ParameterSets { get; }
        public double PitchAdjustmentRatio => GetCurrentPitchAdjustmentRatio();
        public List<Tuple<double, double>> WinningParameterSets { get; }

        /// <summary>
        /// Creates a new instance of <see cref="DynamicParameterProvider"/>
        /// </summary>
        /// <param name="parameterSetListCapacity">Parameter set list capacity</param>
        public DynamicParameterProvider(int parameterSetListCapacity)
        {
            _random = RandomFactory.GetInstance();
            _currentParameterSetIndex = 0;

            ParameterSets = GenerateRandomParameters(parameterSetListCapacity);
            WinningParameterSets = new List<Tuple<double, double>>();
        }

        public Tuple<double, double> GetCurrentParameterSet()
        {
            return ParameterSets[_currentParameterSetIndex];
        }

        public void MarkCurrentParametersAsWinning()
        {
            var winningParameterSet = ParameterSets[_currentParameterSetIndex];

            WinningParameterSets.Add(winningParameterSet);

            ResetParameterSetsIfNecessary();
        }

        protected double GetCurrentHarmonyMemoryConsiderationRatio()
        {
            _currentParameterSetIndex++;
            ResetParameterSetsIfNecessary();

            return GetCurrentParameterSet().Item1;
        }

        protected double GetCurrentPitchAdjustmentRatio()
        {
            return GetCurrentParameterSet().Item2;
        }

        private List<Tuple<double, double>> GenerateRandomParameters(int capacity)
        {
            var parameters = new List<Tuple<double, double>>(capacity);

            for (var i = 0; i < parameters.Capacity; i++)
            {
                var parameterSet = GetRandomParameterSet();
                parameters.Add(parameterSet);
            }

            return parameters;
        }

        private Tuple<double, double> GetRandomParameterSet()
        {
            var harmonyMemoryConsiderationRatio = _random.NextValue(0.9, 1);
            var pitchAdjustmentRatio = _random.NextDouble();

            return new Tuple<double, double>(harmonyMemoryConsiderationRatio, pitchAdjustmentRatio);
        }

        private void ResetParameterSets()
        {
            _currentParameterSetIndex = 0;

            // Use the same parameter set if there are no winning parameters
            if (!WinningParameterSets.Any())
            {
                return;
            }

            ParameterSets.Clear();

            var capacity = ParameterSets.Capacity;
            for (var i = 0; i < capacity; i++)
            {
                var randomValue = _random.NextDouble();
                if (randomValue < 0.75)
                {
                    var winningParameterSet = WinningParameterSets.GetRandomElement();
                    ParameterSets.Add(winningParameterSet);

                    WinningParameterSets.Remove(winningParameterSet);
                }
                else
                {
                    var randomParameterSet = GetRandomParameterSet();
                    ParameterSets.Add(randomParameterSet);
                }
            }

            WinningParameterSets.Clear();
        }

        private void ResetParameterSetsIfNecessary()
        {
            if (_currentParameterSetIndex >= ParameterSets.Count - 1)
            {
                ResetParameterSets();
            }
        }
    }
}