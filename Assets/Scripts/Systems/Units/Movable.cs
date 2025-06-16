using System;
using System.Collections;
using System.Collections.Generic;
using Systems.Movement;
using UnityEditor;
using UnityEngine;

namespace Systems.Units
{
    [RequireComponent(typeof(UnitAnimator))]
    public class Movable : MonoBehaviour, IMovableByPath
    {
        public Queue<Vertex> Path { get; private set; }
        private Queue<Vertex> _bufferPath;

        [field: SerializeField]
        public Vector3 Direction { get; private set; }

        [field: SerializeField]
        public Vertex CurrentWaypoint { get; set; }
        [field: SerializeField]
        public Vertex NextWaypoint { get; private set; }

        [field: SerializeField]
        public bool NeedStop { get; private set; } = false;
        [SerializeField]
        private bool _isMoving;
        public bool IsMoving
        {
            get => _isMoving;
            private set
            {
                _isMoving = value;
                _animator.OnMovingStateChanched?.Invoke(_isMoving, Direction);
            }
        }
        [field: SerializeField]
        public bool OnBridge { get; private set; } = false;
        bool _wasOnBridge = false;

        public float MoveSpeed { get; private set; }
        public float RotationSpeed { get; private set; }
        public float MaxDistanceToWaypoint { get; private set; }

        private Coroutine _moveCoroutine;

        [field: SerializeField]
        public MarkerParent TempParent { get; private set; }

        private Action _onNewPathGot = null;
        private Action _onPlayerStop = null;

        public Action<Vertex> OnWaypointChanged { get; set; } = null;
        public Action OnEnterToBridge { get; private set; } = null;

        private UnitAnimator _animator;

        public void Initialize(float moveSpeed, float rotationSpeed, float maxDistanceToWaypoint)
        {
            _isMoving = false;
            MoveSpeed = moveSpeed;
            RotationSpeed = rotationSpeed;
            MaxDistanceToWaypoint = maxDistanceToWaypoint;

            _animator = GetComponent<UnitAnimator>();

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
                Direction = (NextWaypoint.transform.position - transform.position).normalized;
                IsMoving = true;

                if (Vector3.Distance(CurrentWaypoint.transform.position, NextWaypoint.transform.position) > MaxDistanceToWaypoint)
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

                // ïîâîðà÷èâàòü òîëüêî ïðè äâèæåíèè ïî ïëîñêîé ïîâåðõíîñòè
                // óãîë ìåæäó Y == 90 -- äâèæåíèå ïî ïëîñêîé ïîâåðõíîñòè
                float angleBetweenUp = Vector3.Angle(Direction, Vector3.up);
                if (angleBetweenUp > 80f && angleBetweenUp < 100f)
                {
                    transform.localRotation = Quaternion.Slerp(transform.localRotation,
                        Quaternion.LookRotation(Direction), Time.deltaTime * RotationSpeed);
                }

                // rotate character transform smoothly
                //transform.localRotation = Quaternion.Slerp(transform.localRotation,
                //    Quaternion.LookRotation(Direction), Time.deltaTime * RotationSpeed);
                // rotate character transform immediatly
                //transform.localRotation = Quaternion.LookRotation(Direction);

                if (Vector3.Distance(transform.position, NextWaypoint.transform.position) > MoveSpeed / 2f)
                {
                    transform.localPosition += Direction * MoveSpeed * Time.deltaTime;

                    //IsMoving = true;
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