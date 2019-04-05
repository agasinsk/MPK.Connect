﻿using System;
using System.Collections.Generic;
using System.Linq;
using MPK.Connect.Service.Business.HarmonySearch.Core;
using MPK.Connect.Service.Business.HarmonySearch.Functions;
using MPK.Connect.Service.Business.HarmonySearch.Generator;

namespace MPK.Connect.Console
{
    public class HarmonySearchTestScenario<T> where T : class
    {
        private readonly IEnumerable<HarmonyGeneratorType> _harmonyGeneratorTypes;
        private readonly IEnumerable<HarmonySearchType> _harmonySearchTypes;
        private readonly IEnumerable<ObjectiveFunctionTypes> _objectiveFunctionTypes;
        public List<HarmonySearchTestSettings<T>> Settings { get; set; }

        public HarmonySearchTestScenario()
        {
            _harmonyGeneratorTypes = Enum.GetValues(typeof(HarmonyGeneratorType)).Cast<HarmonyGeneratorType>().ToList();
            _objectiveFunctionTypes = Enum.GetValues(typeof(ObjectiveFunctionTypes)).Cast<ObjectiveFunctionTypes>().ToList();
            _harmonySearchTypes = Enum.GetValues(typeof(HarmonySearchType)).Cast<HarmonySearchType>().ToList();

            Settings = CreateDefaultSettings();
        }

        private List<HarmonySearchTestSettings<T>> CreateDefaultSettings()
        {
            var settings = new List<HarmonySearchTestSettings<T>>();

            foreach (var harmonyGeneratorType in _harmonyGeneratorTypes)
            {
                foreach (var harmonySearcherType in _harmonySearchTypes)
                {
                    foreach (var functionType in _objectiveFunctionTypes)
                    {
                        var testSettings = new HarmonySearchTestSettings<T>
                        {
                            HarmonyGeneratorType = harmonyGeneratorType,
                            HarmonySearcherType = harmonySearcherType,
                            ObjectiveFunctionType = functionType
                        };

                        settings.Add(testSettings);
                    }
                }
            }

            return settings;
        }
    }
}