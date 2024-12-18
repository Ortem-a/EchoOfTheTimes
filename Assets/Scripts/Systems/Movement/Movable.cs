using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Movement
{
    public class Movable : MonoBehaviour
    {
        private Queue<Vertex> _path;

        private Vector3 _direction;

        public Vertex NextWaypoint;

        private float _speed = 0.01f;

        private Coroutine _moveCoroutine;

        public void MoveBy(List<Vertex> path)
        {
            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);

            path.Reverse();
            path.Add(NextWaypoint);

            _path = new Queue<Vertex>(path);

            _moveCoroutine = StartCoroutine(Move());
        }

        private IEnumerator Move()
        {
            do
            {
                _direction = (NextWaypoint.transform.position - transform.position).normalized;

                if (Vector3.Distance(transform.position, NextWaypoint.transform.position) > _speed / 2f)
                {
                    transform.position += _direction * _speed;
                }
                else
                {
                    NextWaypoint = _path.Dequeue();
                }

                yield return null;
            }
            while (_path.Count > 0);
        }

        private void OnDrawGizmos()
        {
            if (_direction == default) return;
            DrawArrow(transform.position, _direction, Color.magenta);
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