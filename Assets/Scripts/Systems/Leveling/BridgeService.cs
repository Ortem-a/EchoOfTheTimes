using UnityEngine;

namespace Systems.Leveling
{
    public class BridgeService : MonoBehaviour
    {
        public Bridge[] Bridges;

        public void Connect()
        {
            for (int i = 0; i < Bridges.Length; i++)
            {
                Bridges[i].Connect();
            }
        }

        public void Disconnect()
        {
            for (int i = 0; i < Bridges.Length; i++)
            {
                Bridges[i].Disconnect();
            }
        }
    }
}