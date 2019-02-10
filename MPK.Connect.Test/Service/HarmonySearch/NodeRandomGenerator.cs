using System;
using MPK.Connect.Service.Business.HarmonySearch.Helpers;
using TspLibNet.Graph.Nodes;

namespace MPK.Connect.Test.Service.HarmonySearch
{
    public class NodeRandomGenerator : Random, IRandomGenerator<INode>
    {
        public INode NextValue(INode minValue, INode maxValue)
        {
            return new Node((minValue.Id + maxValue.Id) / 2);
        }
    }
}