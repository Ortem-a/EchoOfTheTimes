using EchoOfTheTimes.Core;
using EchoOfTheTimes.Effects;
using EchoOfTheTimes.Interfaces;
using EchoOfTheTimes.UI;
using EchoOfTheTimes.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.LevelStates
{
    public class LevelButton : MonoBehaviour, ISpecialVertex
    {
        public List<MovableByButton> Movables;

        public bool IsPressed { get; private set; } = false;

        public Action OnEnter => Press;
        public Action OnExit => null;

        [Header("Camera Shake Settings")]
        [SerializeField] private float shakeIntensity = 0.12f;
        [SerializeField] private float shakeFrequency = 1f;
        [SerializeField] private float shakeDuration = 1f;
        [SerializeField] private float shakeFalloff = 0.8f;
        [SerializeField] private float shakeRandomness = 1f;
        [SerializeField] private bool shakeOnXAxis = true;
        [SerializeField] private bool shakeOnYAxis = true;
        [SerializeField] private float shakeDelay = 0f;

        private GraphVisibility _graph;
        private Player _player;
        private int _maxMovables;
        private int _counter;
        private CameraShake _cameraShake;
        private LevelAudioManager _audioManager;
        private HUDController _hudController;

        [Inject]
        private void Construct(GraphVisibility graph, Player player, CameraShake cameraShake, LevelAudioManager audioManager, HUDController hudController)
        {
            _graph = graph;
            _player = player;
            _cameraShake = cameraShake;
            _audioManager = audioManager;
            _hudController = hudController;
        }

        private void Start()
        {
            _maxMovables = Movables != null ? Movables.Count : 0;
            _counter = 0;

            if (_audioManager != null)
            {
                Debug.Log("LevelAudioManager successfully injected.");
            }
            else
            {
                Debug.LogError("Failed to inject LevelAudioManager.");
            }
        }

        private void Press()
        {
            if (IsPressed) return;

            IsPressed = true;

            Debug.Log($"[LevelButton] {name} pressed! IsPressed: {IsPressed}");

            if (_audioManager != null)
            {
                // Воспроизводим звуки нажатия кнопки и изменения состояния
                _audioManager.PlayButtonPilinkSound();
                _audioManager.PlayButtonChangeSound();
            }
            else
            {
                Debug.LogError("LevelAudioManager is not available.");
            }

            if (_cameraShake == null)
            {
                Debug.LogError("[LevelButton] CameraShake is not assigned!");
                return;
            }

            _hudController.DisableButtons();

            _cameraShake.ShakeCamera(shakeIntensity, shakeFrequency, shakeDuration, shakeFalloff, shakeRandomness, shakeOnXAxis, shakeOnYAxis, shakeDelay);

            _graph.ResetVertices();

            _player.StopAndLink(onComplete: () =>
            {
                for (int i = 0; i < _maxMovables; i++)
                {
                    Movables[i].Move(onComplete: ExecutePostActions);
                }
            });
        }

        private void ExecutePostActions()
        {
            _counter++;

            if (_counter == _maxMovables)
            {
                _graph.Load();

                _player.ForceUnlink();

                _hudController.EnableButtons();
            }
        }
    }
}
