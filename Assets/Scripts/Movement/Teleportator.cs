using DG.Tweening;
using EchoOfTheTimes.Effects;
using EchoOfTheTimes.Interfaces;
using EchoOfTheTimes.ScriptableObjects.Level;
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
        private LevelAudioManager _audioManager;

        private float _teleportDuration_sec;
        private float _teleportDisappearDuration_sec;

        [Inject]
        private void Construct(Player player, LevelSettingsScriptableObject levelSettings, UiSceneController sceneController, LevelAudioManager audioManager)
        {
            _player = player;
            _sceneController = sceneController;
            _audioManager = audioManager;

            _teleportDuration_sec = levelSettings.TeleportDuration_sec;
            _teleportDisappearDuration_sec = levelSettings.TeleportDisappearDuration_sec;
        }

        private void Teleport()
        {
            Debug.Log($"[Teleportator] Teleport to {Destination.transform.position}");

            _player.Stop(() =>
            {
                OnStartTeleportation(() =>
                {
                    _audioManager.PlayTeleportSound();
                    _player.Teleportate(
                        Destination.transform.position,
                        _teleportDuration_sec,
                        onComplete: OnCompleteTeleportation);
                });
            });
        }

        private void OnStartTeleportation(TweenCallback onComplete)
        {
            _sceneController.SetActiveBottomPanel(false);

            _player.transform.DOScale(0f, _teleportDisappearDuration_sec)
                .OnComplete(onComplete);
        }

        private void OnCompleteTeleportation()
        {
            _player.transform.DOScale(1f, _teleportDisappearDuration_sec);

            _sceneController.SetActiveBottomPanel(true);
        }
    }
}
