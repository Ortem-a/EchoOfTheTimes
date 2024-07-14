using EchoOfTheTimes.Core;
using EchoOfTheTimes.Interfaces;
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
        [SerializeField] private float shakeFalloff = 0.8f; // Затухание
        [SerializeField] private float shakeRandomness = 1f;
        [SerializeField] private bool shakeOnXAxis = true; // Тряска по оси X
        [SerializeField] private bool shakeOnYAxis = true; // Тряска по оси Y
        [SerializeField] private float shakeDelay = 0f; // Задержка перед началом тряски

        private GraphVisibility _graph;
        private Player _player;
        private int _maxMovables;
        private int _counter;
        private CameraShake _cameraShake;

        [Inject]
        private void Construct(GraphVisibility graph, Player player, CameraShake cameraShake)
        {
            _graph = graph;
            _player = player;
            _cameraShake = cameraShake;

            _maxMovables = Movables != null ? Movables.Count : 0;
            _counter = 0;
        }

        private void Press()
        {
            if (IsPressed) return;

            IsPressed = true;

            Debug.Log($"[LevelButton] {name} pressed! IsPressed: {IsPressed}");

            if (_cameraShake == null)
            {
                Debug.LogError("[LevelButton] CameraShake is not assigned!");
                return;
            }

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
            }
        }
    }
}
