using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Core
{
    public class SubGraphVisibility : GraphVisibility
    {
        [SerializeField]
        private List<SubGraphBridge> _bridges;

        public void MakeAllBridges()
        {
            for (int i = 0; i < _bridges.Count; i++)
            {
                MakeBridge(_bridges[i]);
            }
        }

        public void BreakAllBridges()
        {
            for (int i = 0; i < _bridges.Count; i++)
            {
                BreakBridge(_bridges[i]);
            }
        }

        private void MakeBridge(SubGraphBridge bridge) => bridge.Connect(MaxDistanceToNeighbourVertex);

        private void BreakBridge(SubGraphBridge bridge) => bridge.Disconnect();
    }
}