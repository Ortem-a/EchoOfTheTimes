using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using EchoOfTheTimes.Animations;
using EchoOfTheTimes.Core;
using EchoOfTheTimes.Interfaces;
using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Movement;
using EchoOfTheTimes.Persistence;
using EchoOfTheTimes.ScriptableObjects;
using EchoOfTheTimes.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace EchoOfTheTimes.Units
{
    [RequireComponent(typeof(AnimationManager))]
    public class Player : MonoBehaviour, IBind<PlayerData>
    {
        [field: SerializeField]
        public SerializableGuid Id { get; set; } = SerializableGuid.NewGuid();
        [SerializeField]
        private PlayerData _data;

        public AnimationManager Animations =>
            _animationManager = _animationManager != null ? _animationManager : GetComponent<AnimationManager>();

        public bool IsBusy { get; set; } = false;

        public Vertex Position => _graph.GetNearestVertex(transform.position);

        private GraphVisibility _graph;
        private VertexFollower _vertexFollower;
        private PlayerSettingsScriptableObject _playerSettings;

        private AnimationManager _animationManager;

        private bool _isNeedLink = false;
        private bool _isNeedStop = false;

        private TweenerCore<Vector3, Path, PathOptions> _pathTweener;

        private Action _onPlayerStop;

        private Movable _movable;

        private void Awake()
        {
            _movable = GetComponent<Movable>();
            _movable.Initialize(1f, 0.05f);
        }

        public void Initialize()
        {
            _graph = GameManager.Instance.Graph;
            _vertexFollower = GameManager.Instance.VertexFollower;
            _playerSettings = GameManager.Instance.PlayerSettings;
        }

        public void Teleportate(Vector3 to, float duration, TweenCallback onStart = null, TweenCallback onComplete = null)
        {
            Debug.Log($"[TeleportTo] {to}");

            transform.DOMove(to, duration)
                .SetEase(Ease.Linear)
                .OnStart(onStart)
                .OnComplete(onComplete);
        }

        public void MoveTo(Vector3[] waypoints)
        {
            _movable.Move(waypoints, OnStartExecution, OnCompleteExecution);

            //OnStartExecution();

            //transform.DOLookAt(waypoints[0], _playerSettings.RotateDuration, _playerSettings.AxisConstraint);

            //_pathTweener = transform.DOPath(
            //    path: waypoints,
            //    duration: _playerSettings.MoveDuration * waypoints.Length,
            //    pathType: _playerSettings.PathType,
            //    pathMode: _playerSettings.PathMode,
            //    gizmoColor: _playerSettings.GizmoColor
            //    )
            //    .OnWaypointChange((x) =>
            //        {
            //            if (x != 0)
            //            {
            //                OnCompleteExecution();
            //                if (x < waypoints.Length)
            //                {
            //                    transform.DOLookAt(waypoints[x], _playerSettings.RotateDuration, _playerSettings.AxisConstraint);
            //                }
            //            }
            //        })
            //    .SetEase(_playerSettings.Ease);
        }

        private void OnStartExecution()
        {
            IsBusy = true;

            if (Position.gameObject.TryGetComponent(out StateFreezer freezer))
            {
                freezer.OnCancel?.Invoke();
            }

            if (Position.gameObject.TryGetComponent(out ISpecialVertex specialVertex))
            {
                specialVertex.OnExit?.Invoke();
            }
        }

        private void OnCompleteExecution()
        {
            IsBusy = false;

            if (Position.gameObject.TryGetComponent(out StateFreezer freezer))
            {
                freezer.OnFreeze?.Invoke();
            }

            if (Position.gameObject.TryGetComponent(out ISpecialVertex specialVertex))
            {
                specialVertex.OnEnter?.Invoke();
            }

            if (_isNeedLink)
            {
                ForceStopAndLink();
            }

            if (_isNeedStop)
            {
                ForceStop();
            }
        }

        public void StopAndLink(Action onComplete)
        {
            _isNeedLink = true;

            _onPlayerStop = onComplete;

            if (!IsBusy)
            {
                ForceStopAndLink();
            }
        }

        private void ForceStopAndLink()
        {
            _isNeedLink = false;

            // ������� � ForceLink() ��� ������
            // ��� ��-�� StateFreezer'a
            //_vertexFollower.OnAcceptLink?.Invoke();

            ForceStop();
        }

        public void ForceLink()
        {
            _vertexFollower.OnAcceptLink?.Invoke();
        }

        public void Stop(Action onComplete)
        {
            _isNeedStop = true;

            _onPlayerStop = onComplete;

            if (!IsBusy)
            {
                ForceStop();
            }
        }

        private void ForceStop()
        {
            _movable.Stop();

            _isNeedStop = false;

            _pathTweener.Kill();

            _onPlayerStop?.Invoke();
        }

        public void Bind(PlayerData data)
        {
            _data = data;
            _data.Id = Id;

            //var vertex = _graph.GetNearestVertex(data.Checkpoint);

            //_levelStateMachine.LoadState(data.StateId);
            //_checkpointManager.OnCheckpointChanged?.Invoke(vertex.gameObject.GetComponent<Checkpoint>());
        }
    }
}