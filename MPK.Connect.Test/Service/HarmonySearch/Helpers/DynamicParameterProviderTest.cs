using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPK.Connect.Service.Business.HarmonySearch.ParameterProviders;

namespace MPK.Connect.Test.Service.HarmonySearch.Helpers
{
    [TestClass]
    public class DynamicParameterProviderTest
    {
        private readonly DynamicParameterProvider _parameterProvider;

        public DynamicParameterProviderTest()
        {
            _parameterProvider = new DynamicParameterProvider(5);
        }

        [TestMethod]
        public void MarkCurrentParametersAsWinning_ShouldMoveAllParameters_AndClearTheList()
        {
            // Arrange
            var capacityBeforeMarking = _parameterProvider.ParameterSets.Capacity;

            var parameters = new List<Tuple<double, double>>();

            for (int i = 0; i < _parameterProvider.ParameterSets.Count - 1; i++)
            {
                parameters.Add(_parameterProvider.GetCurrentParameterSet());
                _parameterProvider.MarkCurrentParametersAsWinning();
            }

            parameters.Add(_parameterProvider.GetCurrentParameterSet());

            // Act
            _parameterProvider.MarkCurrentParametersAsWinning();

            // Assert
            Assert.AreEqual(capacityBeforeMarking, _parameterProvider.ParameterSets.Count);
            Assert.AreEqual(0, _parameterProvider.WinningParameterSets.Count);
        }

        [TestMethod]
        public void MarkCurrentParametersAsWinning_ShouldMoveTheParameterToWinningSet()
        {
            // Arrange
            var countBeforeMarking = _parameterProvider.ParameterSets.Count;
            var winningCountBeforeMarking = _parameterProvider.WinningParameterSets.Count;
            var parameterSet = _parameterProvider.GetCurrentParameterSet();

            // Act
            _parameterProvider.MarkCurrentParametersAsWinning();

            // Assert
            Assert.AreEqual(countBeforeMarking, _parameterProvider.ParameterSets.Count);
            Assert.AreEqual(winningCountBeforeMarking + 1, _parameterProvider.WinningParameterSets.Count);
            Assert.IsTrue(_parameterProvider.WinningParameterSets.Contains(parameterSet));
        }
    }
}