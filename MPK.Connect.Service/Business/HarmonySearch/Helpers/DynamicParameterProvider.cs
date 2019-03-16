using System;
using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Service.Helpers;
using MPK.Connect.Service.Utils;

namespace MPK.Connect.Service.Business.HarmonySearch.Helpers
{
    /// <summary>
    /// Contains logic for providing dynamic parameter (Winning Parameter Set List)
    /// </summary>
    public class DynamicParameterProvider
    {
        private readonly IBoundedRandom _random;
        private int _currentParameterSetIndex;
        public List<Tuple<double, double>> ParameterSets { get; }
        public List<Tuple<double, double>> WinningParameterSets { get; }

        public DynamicParameterProvider(int capacity)
        {
            _random = RandomFactory.GetInstance();
            _currentParameterSetIndex = -1;

            ParameterSets = GenerateRandomParameters(capacity);
            WinningParameterSets = new List<Tuple<double, double>>();
        }

        public Tuple<double, double> GetParameterSet()
        {
            _currentParameterSetIndex++;
            var parameterSet = ParameterSets[_currentParameterSetIndex];

            return parameterSet;
        }

        public void MarkCurrentParametersAsWinning()
        {
            var parameterSet = ParameterSets[_currentParameterSetIndex];
            WinningParameterSets.Add(parameterSet);

            if (_currentParameterSetIndex >= ParameterSets.Count - 1)
            {
                ResetParameterSets();
            }
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
            var parameterSet = new Tuple<double, double>(harmonyMemoryConsiderationRatio, pitchAdjustmentRatio);

            return parameterSet;
        }

        private void ResetParameterSets()
        {
            _currentParameterSetIndex = 0;

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
    }
}