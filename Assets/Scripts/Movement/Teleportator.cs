using DG.Tweening;
using EchoOfTheTimes.Core;
using EchoOfTheTimes.Interfaces;
using EchoOfTheTimes.Units;
using System;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Movement
{
    public class Teleportator : MonoBehaviour, ISpecialVertex
    {
        private Player _player;

        public Teleportator Destination;

        public Action OnEnter => Teleport;
        public Action OnExit => null;

        [Inject]
        private void Initialize(Player player)
        {
            _player = player;
        }

        public void Initialize()
        {
            _player = GameManager.Instance.Player;
        }

        private void Teleport()
        {
            Debug.Log($"[Teleportator] Teleport to {Destination.transform.position}");

            _player.Teleportate(
                Destination.transform.position,
                GameManager.Instance.TeleportDuration_sec,
                onStart: OnStartTeleportation,
                onComplete: OnCompleteTeleportation); ;
        }

        private void OnStartTeleportation()
        {
            _player.transform.DOScale(0f, GameManager.Instance.TeleportDisappearDuration_sec);
        }

        private void OnCompleteTeleportation()
        {
            _player.transform.DOScale(1f, GameManager.Instance.TeleportDisappearDuration_sec);
        }
    }
}