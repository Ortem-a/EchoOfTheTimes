using EchoOfTheTimes.Core;
using EchoOfTheTimes.Interfaces;
using EchoOfTheTimes.Units;
using System;
using UnityEngine;

namespace EchoOfTheTimes.Movement
{
    public class Teleportator : MonoBehaviour, ISpecialVertex
    {
        private Player _player;

        public Teleportator Destination;

        public Action OnEnter => Teleport;

        public Action OnExit => null;

        public void Initialize()
        {
            _player = GameManager.Instance.Player;
        }

        private void Teleport()
        {
            Debug.Log($"[Teleportator] Teleport to {Destination.transform.position}");

            _player.TeleportTo(Destination.transform.position);
        }
    }
}