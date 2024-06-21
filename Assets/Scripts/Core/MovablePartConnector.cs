using System.Collections.Generic;

namespace EchoOfTheTimes.Core
{
    [System.Serializable]
    public class MovablePartConnector
    {
        [field: UnityEngine.SerializeField]
        public List<MovablePartBridge> Bridges { get; private set; }

        [UnityEngine.SerializeField]
        private float _maxDistanceToNeighbour;

        public void MakeAllBridges()
        {
            for (int i = 0; i < Bridges.Count; i++)
            {
                MakeBridge(Bridges[i]);
            }
        }

        public void BreakAllBridges()
        {
            for (int i = 0; i < Bridges.Count; i++)
            {
                BreakBridge(Bridges[i]);
            }
        }

        private void MakeBridge(MovablePartBridge bridge) => bridge.Connect(_maxDistanceToNeighbour);

        private void BreakBridge(MovablePartBridge bridge) => bridge.Disconnect();
    }
}