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

        public Vertex CurrentWaypoint;
        public Vertex NextWaypoint;

        [SerializeField]
        private bool _needStop = false;
        [SerializeField]
        private bool _isMoving = false;
        private float _speed = 0.01f;

        private Coroutine _moveCoroutine;

        [SerializeField]
        private DummyParent _tempParent;

        private Action _onNewPathGot;

        public void MoveBy(List<Vertex> path)
        {
            if (path.Count != 0)
            {
                path.Reverse();
                _bufferPath = new Queue<Vertex>(path);

                _onNewPathGot = HandleNewPath;

                if (_isMoving)
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
            _needStop = true;
        }

        private IEnumerator Move()
        {
            do
            {
                _direction = (NextWaypoint.transform.position - transform.position).normalized;

                if (Vector3.Distance(transform.position, NextWaypoint.transform.position) > _speed / 2f)
                {
                    transform.localPosition += _direction * _speed;

                    _isMoving = true;
                }
                else
                {
                    CurrentWaypoint = NextWaypoint;
                    SetParent(CurrentWaypoint);

                    if (_needStop)
                    {
                        _isMoving = false;
                        _needStop = false;

                        NextWaypoint = null;

                        _onNewPathGot?.Invoke();
                    }
                    else
                    {
                        _path.TryDequeue(out NextWaypoint);
                    }
                }

                yield return null;
            }
            while (NextWaypoint != null);

            _isMoving = false;
        }

        private void SetParent(Vertex vertex)
        {
            var newDummy = GetParentRecursively(vertex.transform);

            if (newDummy == null)
            {
                _tempParent = null;
                transform.SetParent(null);
            }
            else if (!ReferenceEquals(_tempParent, newDummy))
            {
                _tempParent = newDummy;
                transform.SetParent(_tempParent.transform);
            }
        }

        private DummyParent GetParentRecursively(Transform t)
        {
            if (t == null) return null;

            if (t.TryGetComponent<DummyParent>(out var dummy))
            {
                return dummy;
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
                    Gizmos.DrawSphere(v.transform.position, .15f);
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
    }
}