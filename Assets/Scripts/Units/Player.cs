using DG.Tweening;
using EchoOfTheTimes.Effects;
using EchoOfTheTimes.Core;
using EchoOfTheTimes.Interfaces;
using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Movement;
using System;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Units
{
    [RequireComponent(typeof(AnimationManager), typeof(Movable), typeof(PlayerPath)),
    RequireComponent(typeof(SoundManager))]
    public class Player : MonoBehaviour
    {
        public AnimationManager Animations =>
            _animationManager = _animationManager != null ? _animationManager : GetComponent<AnimationManager>();

        public bool IsBusy { get; set; } = false;
        public bool IsTeleportate { get; private set; } = false;

        private Vertex _position;
        public Vertex Position => _position == null ? _graph.GetNearestVertex(transform.position) : _position;

        public Vertex NextPosition => _movable.Destination;

        public bool StayOnDynamic => _playerPath.StayOnDynamic;
        public bool PreviousWaypointIsDynamic => _playerPath.PrevIsDynamic;

        private GraphVisibility _graph;
        private VertexFollower _vertexFollower;
        private PlayerPath _playerPath;
        private Movable _movable;
        private InputMediator _inputMediator;

        private AnimationManager _animationManager;

        private Action _onMoveCompleted = null;

        [Inject]
        private void Construct(GraphVisibility graphVisibility, VertexFollower vertexFollower, Movable movable, PlayerPath playerPath, InputMediator inputMediator)
        {
            _graph = graphVisibility;
            _vertexFollower = vertexFollower;
            _movable = movable;
            _playerPath = playerPath;
            _inputMediator = inputMediator;
        }

        public void Teleportate(Vector3 to, float duration, TweenCallback onStart = null, TweenCallback onComplete = null)
        {
            Debug.Log($"[TeleportTo] {to}");

            ResetNextPosition();

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

        public void MoveTo(Vertex[] waypoints, bool isFictitious = false)
        {
            if (isFictitious)
            {
                _movable.Move(waypoints, OnStartMoveFictitious, OnCompleteMoveFictitious);
            }
            else
            {
                _movable.Move(waypoints, OnStartMove, OnCompleteMove);
            }
        }

        private void OnStartMoveFictitious()
        {
            IsBusy = true;

            _position = _graph.GetNearestVertex(transform.position);

            if (Position.gameObject.TryGetComponent(out StateFreezer freezer))
            {
                freezer.OnCancel?.Invoke();
            }

            if (NextPosition.gameObject.TryGetComponent(out StateFreezer nextFreezer))
            {
                nextFreezer.OnFreeze?.Invoke();
            }
        }

        private void OnStartMove()
        {
            IsBusy = true;

            _position = _graph.GetNearestVertex(transform.position);

            if (Position.gameObject.TryGetComponent(out StateFreezer freezer))
            {
                freezer.OnCancel?.Invoke();
            }

            if (NextPosition.gameObject.TryGetComponent(out StateFreezer nextFreezer))
            {
                nextFreezer.OnFreeze?.Invoke();
            }

            if (Position.gameObject.TryGetComponent(out ISpecialVertex specialVertex))
            {
                specialVertex.OnExit?.Invoke();
            }
        }

        private void OnCompleteMoveFictitious()
        {
            IsBusy = false;

            _onMoveCompleted?.Invoke();
            _onMoveCompleted = null;

            _position = _graph.GetNearestVertex(transform.position);

            if (Position.gameObject.TryGetComponent(out StateFreezer freezer))
            {
                freezer.OnFreeze?.Invoke();
            }

            // Дополнительная логика для фиктивного тача
        }

        private void OnCompleteMove()
        {
            IsBusy = false;

            _onMoveCompleted?.Invoke();
            _onMoveCompleted = null;

            _position = _graph.GetNearestVertex(transform.position);

            if (Position.gameObject.TryGetComponent(out StateFreezer freezer))
            {
                freezer.OnFreeze?.Invoke();
            }

            if (Position.gameObject.TryGetComponent(out ISpecialVertex specialVertex))
            {
                specialVertex.OnEnter?.Invoke();
            }
        }

        public void WaitUntilCompleteMove(Action onComplete)
        {
            _onMoveCompleted += onComplete;
        }

        private void OnStartTeleportate()
        {
            IsTeleportate = true;
            IsBusy = true;

            _position = _graph.GetNearestVertex(transform.position);

            if (Position.gameObject.TryGetComponent(out StateFreezer freezer))
            {
                freezer.OnFreeze?.Invoke();
            }
        }

        private void OnCompleteTeleportate()
        {
            IsTeleportate = false;
            IsBusy = false;

            _position = _graph.GetNearestVertex(transform.position);

            if (Position.gameObject.TryGetComponent(out StateFreezer freezer))
            {
                freezer.OnFreeze?.Invoke();
            }

            // Симуляция касания в финальный вертекс
            _inputMediator.SimulateTouch(_position);
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

        private void ResetNextPosition()
        {
            _movable.ResetDestination();
        }
    }
}
