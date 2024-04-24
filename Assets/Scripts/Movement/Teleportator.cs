using DG.Tweening;
using EchoOfTheTimes.Interfaces;
using EchoOfTheTimes.ScriptableObjects;
using EchoOfTheTimes.UI;
using EchoOfTheTimes.Units;
using System;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Movement
{
    public class Teleportator : MonoBehaviour, ISpecialVertex
    {
        public Teleportator Destination;

        public Action OnEnter => Teleport;
        public Action OnExit => null;

        private Player _player;
        private UiSceneController _sceneController;

        private float _teleportDuration_sec;
        private float _teleportDisappearDuration_sec;

        [Inject]
        private void Construct(Player player, LevelSettingsScriptableObject levelSettings, UiSceneController sceneController)
        {
            _player = player;

            _teleportDuration_sec = levelSettings.TeleportDuration_sec;
            _teleportDisappearDuration_sec = levelSettings.TeleportDisappearDuration_sec;

            _sceneController = sceneController;
        }

        private void Teleport()
        {
            Debug.Log($"[Teleportator] Teleport to {Destination.transform.position}");

#warning �� ����� ����� � �������� ������ �������� �� �����
            _player.Stop(() =>
            {
                _player.Teleportate(
                    Destination.transform.position,
                    _teleportDuration_sec,
                    onStart: OnStartTeleportation,
                    onComplete: OnCompleteTeleportation);
            });
        }

        private void OnStartTeleportation()
        {
            _player.transform.DOScale(0f, _teleportDisappearDuration_sec);

            _sceneController.SetActiveBottomPanel(false);
        }

        private void OnCompleteTeleportation()
        {
            _player.transform.DOScale(1f, _teleportDisappearDuration_sec);

            _sceneController.SetActiveBottomPanel(true);
        }
    }
}