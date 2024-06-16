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
    [RequireComponent(typeof(AnimationManager), typeof(Movable), typeof(PlayerPath))]
    public class Player : MonoBehaviour
    {
        public AnimationManager Animations =>
            _animationManager = _animationManager != null ? _animationManager : GetComponent<AnimationManager>();

        public bool IsBusy { get; set; } = false;

        private Vertex _position;
        public Vertex Position => _position == null ? _graph.GetNearestVertex(transform.position) : _position;

        public Vertex NextPosition => _movable.Destination;

        public bool StayOnDynamic => _playerPath.StayOnDynamic;

        private GraphVisibility _graph;
        private VertexFollower _vertexFollower;
        private PlayerPath _playerPath;
        private Movable _movable;

        private AnimationManager _animationManager;

        [Inject]
        private void Construct(GraphVisibility graphVisibility, VertexFollower vertexFollower, Movable movable, PlayerPath playerPath)
        {
            _graph = graphVisibility;
            _vertexFollower = vertexFollower;
            _movable = movable;
            _playerPath = playerPath;
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
                    onComplete?.Invoke();
                    OnCompleteTeleportate();
                });
        }

        public void MoveTo(Vertex[] waypoints)
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
                //freezer.OnCancel?.Invoke();
                freezer.OnFreeze?.Invoke();
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

        public void CutPath()
        {
            _playerPath.CutPath();
        }
    }
}