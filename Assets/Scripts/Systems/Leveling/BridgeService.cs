using UnityEngine;

namespace Systems.Leveling
{
    public class BridgeService : MonoBehaviour
    {
        public Bridge[] Bridges;

        public bool AllConnected = true;

        public void Connect()
        {
            for (int i = 0; i < Bridges.Length; i++)
            {
                Bridges[i].Connect();
            }

            AllConnected = true;
        }

        public void Disconnect()
        {
            for (int i = 0; i < Bridges.Length; i++)
            {
                Bridges[i].Disconnect();
            }

            AllConnected = false;
        }

        private void OnDrawGizmos()
        {
            if (AllConnected)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.yellow;
            }

            for (int i = 0; i < Bridges.Length; i++)
            {
                if (Bridges[i].Inner == null || Bridges[i].Outer == null) continue;

                Gizmos.DrawWireCube(
                    (Bridges[i].Inner.transform.position + Bridges[i].Outer.transform.position) / 2f,
                    (Bridges[i].Inner.transform.position - Bridges[i].Outer.transform.position) + Vector3.one * 0.1f);
            }
        }
    }
}