using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Movement
{
    public class Movable : MonoBehaviour
    {
        private Queue<Vertex> _path;
        private Queue<Vertex> _bufferPath;

        private Vector3 _direction;

        [field: SerializeField]
        public Vertex CurrentWaypoint { get; set; }
        [field: SerializeField]
        public Vertex NextWaypoint { get; private set; }

        [field: SerializeField]
        public bool NeedStop { get; private set; } = false;
        [field: SerializeField]
        public bool IsMoving { get; private set; } = false;

        private float _speed = 0.01f;

        private Coroutine _moveCoroutine;

        [SerializeField]
        private MarkerParent _tempParent;

        private Action _onNewPathGot;

        public void MoveBy(List<Vertex> path)
        {
            if (path.Count != 0)
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
        }

        private void HandleNewPath()
        {
            _onNewPathGot = null;

            _path = new Queue<Vertex>(_bufferPath);
            NextWaypoint = _bufferPath.Dequeue();

            _bufferPath.Clear();

            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);

            _moveCoroutine = StartCoroutine(Move());
        }

        public void Stop()
        {
            NeedStop = true;
        }

        private IEnumerator Move()
        {
            do
            {
                _direction = (NextWaypoint.transform.position - transform.position).normalized;

                if (Vector3.Distance(transform.position, NextWaypoint.transform.position) > _speed / 2f)
                {
                    transform.localPosition += _direction * _speed;

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

                        _onNewPathGot?.Invoke();
                    }
                    else
                    {
                        _path.TryDequeue(out var nextWaypoint);
                        NextWaypoint = nextWaypoint;
                    }
                }

                yield return null;
            }
            while (NextWaypoint != null);

            IsMoving = false;
        }

        private void SetParent(Vertex vertex)
        {
            var newMarker = GetParentRecursively(vertex.transform);

            if (newMarker == null)
            {
                _tempParent = null;
                transform.SetParent(null);
            }
            else if (!ReferenceEquals(_tempParent, newMarker))
            {
                _tempParent = newMarker;
                transform.SetParent(_tempParent.transform);
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

        private void OnDrawGizmos()
        {
            if (_direction != default)
            {
                DrawArrow(transform.position, _direction, Color.magenta);
            }

            if (_path != null)
            {
                Gizmos.color = Color.blue;

                foreach (Vertex v in _path)
                {
                    Gizmos.DrawSphere(v.transform.position, 0.15f);
                }
            }
        }

        private void DrawArrow(Vector3 position, Vector3 direction, Color color,
            float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            Gizmos.color = color;

            Gizmos.DrawRay(position, direction);

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * Vector3.forward;
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * Vector3.forward;
            Gizmos.DrawRay(position + direction, right * arrowHeadLength);
            Gizmos.DrawRay(position + direction, left * arrowHeadLength);
        }

        public MarkerParent GetMarkerParent() => _tempParent;
    }
}