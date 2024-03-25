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
using EchoOfTheTimes.Utils;
using System;
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

        public float Duration;

        public bool IsBusy { get; set; } = false;

        public Vertex Position => _graph.GetNearestVertex(transform.position);

        private GraphVisibility _graph;
        private VertexFollower _vertexFollower;

        private AnimationManager _animationManager;

        private bool _isNeedLink = false;
        private bool _isNeedStop = false;
        //private Sequence _sequence;
        private TweenerCore<Vector3, Path, PathOptions> _pathTweener;

        private Action _onPlayerStop;

        public void Initialize()
        {
            _graph = GameManager.Instance.Graph;
            _vertexFollower = GameManager.Instance.VertexFollower;
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
            //_sequence = DOTween.Sequence();

            OnStartExecution();

            transform.DOLookAt(waypoints[0], 0.25f, AxisConstraint.Y);

            _pathTweener = transform.DOPath(waypoints, Duration * waypoints.Length)
                .OnWaypointChange((x) =>
                    {
                        if (x != 0)
                        {
                            OnCompleteExecution();
                            if (x < waypoints.Length)
                            {
                                transform.DOLookAt(waypoints[x], 0.25f, AxisConstraint.Y);
                            }
                        }
                    })
                .SetEase(Ease.Linear);

            //foreach (var waypoint in waypoints)
            //{
            //    _sequence.Append(
            //        transform.DOMove(waypoint, Duration)
            //            .SetEase(Ease.Linear)
            //            .OnComplete(OnCompleteExecution)
            //        );
            //}
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
            _isNeedStop = false;

            _pathTweener.Kill();
            //_sequence.Kill();

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