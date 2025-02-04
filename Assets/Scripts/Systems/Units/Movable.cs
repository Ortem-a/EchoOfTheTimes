using System;
using System.Collections;
using System.Collections.Generic;
using Systems.Movement;
using UnityEngine;

namespace Systems.Units
{
    public class Movable : MonoBehaviour, IMovableByPath
    {
        public Queue<Vertex> Path { get; private set; }
        private Queue<Vertex> _bufferPath;

        public Vector3 Direction { get; private set; }

        [field: SerializeField]
        public Vertex CurrentWaypoint { get; set; }
        [field: SerializeField]
        public Vertex NextWaypoint { get; private set; }

        [field: SerializeField]
        public bool NeedStop { get; private set; } = false;
        [field: SerializeField]
        public bool IsMoving { get; private set; } = false;
        [field: SerializeField]
        public bool OnBridge { get; private set; } = false;
        bool _wasOnBridge = false;

        public float Speed { get; private set; }

        private Coroutine _moveCoroutine;

        [field: SerializeField]
        public MarkerParent TempParent { get; private set; }

        private Action _onNewPathGot = null;
        private Action _onPlayerStop = null;

        public Action<Vertex> OnWaypointChanged { get; set; } = null;
        public Action OnEnterToBridge { get; private set; } = null;

        public void Initialize(float speed)
        {
            Speed = speed;

            OnEnterToBridge += HandleEnteringToBridge;
            OnWaypointChanged += HandleNewWaypoint;
        }

        private void OnDestroy()
        {
            OnEnterToBridge -= HandleEnteringToBridge;
            OnWaypointChanged -= HandleNewWaypoint;
        }

        private void HandleEnteringToBridge()
        {
            OnBridge = true;
        }

        private void HandleNewWaypoint(Vertex newVertex)
        {
            OnBridge = false;

            if (NextWaypoint != null)
            {
                if (Vector3.Distance(CurrentWaypoint.transform.position, NextWaypoint.transform.position) > 2f)
                {
                    ForceStop();
                }
                else if (CurrentWaypoint.IsBridge && NextWaypoint.IsBridge)
                {
                    OnEnterToBridge?.Invoke();
                }
            }
        }

        public void MoveBy(List<Vertex> path)
        {
            //if (path.Count != 0)
            //{
                path.Reverse();
                _bufferPath = new Queue<Vertex>(path);

                _onNewPathGot = HandleNewPath;

                if (IsMoving)
                {
                    Stop();
                }
                else
                {
                    _onNewPathGot?.Invoke();
                }
            //}
        }

        private void HandleNewPath()
        {
            _onNewPathGot = null;

            Path = new Queue<Vertex>(_bufferPath);
            NextWaypoint = Path.Dequeue();

            _bufferPath.Clear();

            OnWaypointChanged?.Invoke(CurrentWaypoint);

            if (NextWaypoint == null)
            {
                Debug.Log("NEXT WP IS NULL!!!!");
                return;
            }

            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);

            _moveCoroutine = StartCoroutine(Move());
        }

        public void Stop(Action onStopped = null)
        {
            NeedStop = true;

            _onPlayerStop = onStopped;
        }

        private IEnumerator Move()
        {
            do
            {
                SkipBridgeIfNeed();

                Direction = (NextWaypoint.transform.position - transform.position).normalized;

                if (Vector3.Distance(transform.position, NextWaypoint.transform.position) > Speed / 2f)
                {
                    transform.localPosition += Direction * Speed;

                    IsMoving = true;
                }
                else
                {
                    CurrentWaypoint = NextWaypoint;
                    SetParent(CurrentWaypoint);

                    if (NeedStop)
                    {
                        IsMoving = false;
                        NeedStop = false;

                        NextWaypoint = null;

                        _onPlayerStop?.Invoke();
                        _onPlayerStop = null;

                        _onNewPathGot?.Invoke();
                    }
                    else
                    {
                        Path.TryDequeue(out var nextWaypoint);
                        NextWaypoint = nextWaypoint;
                    }

                    OnWaypointChanged?.Invoke(CurrentWaypoint);

                    CheckForDoubleBridge();
                }

                yield return null;
            }
            while (NextWaypoint != null);

            IsMoving = false;
        }

        private void ForceStop()
        {
            IsMoving = false;
            NeedStop = false;
            OnBridge = false;

            NextWaypoint = null;
        }

        public void StopImmediate()
        {
            ForceStop();
        }

        public void SetParent(Vertex vertex)
        {
            var newMarker = GetParentRecursively(vertex.transform);

            if (newMarker == null)
            {
                TempParent = null;
                transform.SetParent(null);
            }
            else if (!ReferenceEquals(TempParent, newMarker))
            {
                TempParent = newMarker;
                transform.SetParent(TempParent.transform);
            }
        }

        private MarkerParent GetParentRecursively(Transform t)
        {
            if (t == null) return null;

            if (t.TryGetComponent<MarkerParent>(out var marker))
            {
                return marker;
            }

            return GetParentRecursively(t.parent);
        }

        private void SkipBridgeIfNeed()
        {
            if (OnBridge && (NextWaypoint.IsMoving || CurrentWaypoint.IsMoving))
            {
                _wasOnBridge = true;
                transform.position = NextWaypoint.transform.position;
            }
        }

        private void CheckForDoubleBridge()
        {
            if (OnBridge && _wasOnBridge)
            {
                ForceStop();
            }

            _wasOnBridge = false;
        }
    }
}