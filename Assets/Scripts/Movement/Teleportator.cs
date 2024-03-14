using EchoOfTheTimes.Core;
using UnityEngine;

namespace EchoOfTheTimes.Movement
{
    public class Teleportator : MonoBehaviour
    {
        public Teleportator Destination;

        public void Teleport()
        {
            Debug.Log($"[Teleportator] Teleport to {Destination.transform.position}");

            GameManager.Instance.Player.TeleportTo(Destination.transform.position);
        }
    }
}