using DG.Tweening;
using EchoOfTheTimes.Animations;
using EchoOfTheTimes.Core;
using EchoOfTheTimes.Interfaces;
using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Movement;
using EchoOfTheTimes.ScriptableObjects;
using System;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Units
{
    [RequireComponent(typeof(AnimationManager), typeof(Movable))]
    public class Player : MonoBehaviour
    {
        public AnimationManager Animations =>
            _animationManager = _animationManager != null ? _animationManager : GetComponent<AnimationManager>();

        public bool IsBusy { get; set; } = false;

        private Vertex _position;
        public Vertex Position => _position == null ? _graph.GetNearestVertex(transform.position) : _position;

        private GraphVisibility _graph;
        private VertexFollower _vertexFollower;
        private PlayerSettingsScriptableObject _playerSettings;

        private AnimationManager _animationManager;

        private Movable _movable;

        [Inject]
        private void Construct(GraphVisibility graphVisibility, VertexFollower vertexFollower, PlayerSettingsScriptableObject playerSettings)
        {
            _graph = graphVisibility;
            _vertexFollower = vertexFollower;
            _playerSettings = playerSettings;

            _movable = GetComponent<Movable>();
            _movable.Initialize(
                speed: _playerSettings.MoveSpeed,
                distanceTreshold: _playerSettings.DistanceTreshold,
                rotateDuration: _playerSettings.RotateDuration,
                rotateConstraint: _playerSettings.AxisConstraint
                );
        }

        public void Teleportate(Vector3 to, float duration, TweenCallback onStart = null, TweenCallback onComplete = null)
        {
            Debug.Log($"[TeleportTo] {to}");

            transform.DOMove(to, duration)
                .SetEase(Ease.Linear)
                .OnStart(() =>
                {
                    OnStartTeleportate();
                    onStart?.Invoke();
                })
                .OnComplete(() =>
                {
                    OnCompleteTeleportate();
                    onComplete?.Invoke();
                });
        }

        public void MoveTo(Vector3[] waypoints)
        {
            _movable.Move(waypoints, OnStartMove, OnCompleteMove);
        }

        private void OnStartMove()
        {
            IsBusy = true;

            _position = _graph.GetNearestVertex(transform.position);

            //Debug.Log($"[ON START MOVE] {_position}");

            if (Position.gameObject.TryGetComponent(out StateFreezer freezer))
            {
                freezer.OnCancel?.Invoke();
            }

            if (Position.gameObject.TryGetComponent(out ISpecialVertex specialVertex))
            {
                specialVertex.OnExit?.Invoke();
            }
        }

        private void OnCompleteMove()
        {
            IsBusy = false;

            _position = _graph.GetNearestVertex(transform.position);

            //Debug.Log($"[ON COMPLETE MOVE] {_position}");

            if (Position.gameObject.TryGetComponent(out StateFreezer freezer))
            {
                freezer.OnFreeze?.Invoke();
            }

            if (Position.gameObject.TryGetComponent(out ISpecialVertex specialVertex))
            {
                specialVertex.OnEnter?.Invoke();
            }
        }

        private void OnStartTeleportate()
        {
            IsBusy = true;

            _position = _graph.GetNearestVertex(transform.position);

            //Debug.Log($"[ON START TELEPORTATE] {_position}");

            if (Position.gameObject.TryGetComponent(out StateFreezer freezer))
            {
                freezer.OnCancel?.Invoke();
            }
        }

        private void OnCompleteTeleportate()
        {
            IsBusy = false;

            _position = _graph.GetNearestVertex(transform.position);

            //Debug.Log($"[ON COMPLETE TELEPORTATE] {_position}");

            if (Position.gameObject.TryGetComponent(out StateFreezer freezer))
            {
                freezer.OnFreeze?.Invoke();
            }
        }

        public void StopAndLink(Action onComplete)
        {
            _movable.Stop(onStopped: () =>
            {
                _vertexFollower.OnAcceptLink?.Invoke();
                onComplete?.Invoke();
            });
        }

        public void Stop(Action onComplete)
        {
            _movable.Stop(onStopped: onComplete);
        }

        public void ForceUnlink()
        {
            _vertexFollower.Unlink();
        }
    }
}