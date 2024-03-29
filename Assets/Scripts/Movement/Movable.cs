using DG.Tweening;
using System;
using UnityEngine;

namespace EchoOfTheTimes.Movement
{
    public class Movable : MonoBehaviour
    {
        private float _speed;
        private float _distanceTreshold;
        private int _waypointIndex;
        private Vector3 _destination;
        private Vector3[] _path;

        private bool _isMoving = false;
        private bool _isNeedToStop = false;

        private Action _onStartMoving;
        private Action _onCompleteMoving;

        private void Update()
        {
            if (_isMoving) 
            {
                if (Vector3.Distance(transform.position, _destination) < _distanceTreshold)
                {
                    _onCompleteMoving?.Invoke();

                    if (!TryGetNextWaypoint(out _destination))
                    {
                        ForceStop();
                    }
                    else
                    {
                        transform.DOLookAt(_destination, 0.1f, AxisConstraint.Y);

                        _onStartMoving?.Invoke();
                    }

                    if (_isNeedToStop) 
                    {
                        ForceStop();
                        _isNeedToStop = false;
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, _destination, _speed * Time.deltaTime);
                }
            }
        }

        private bool TryGetNextWaypoint(out Vector3 destination)
        {
            destination = Vector3.zero;
            _waypointIndex++;

            if (_waypointIndex < _path.Length) 
            {
                destination = _path[_waypointIndex];
                return true;
            }

            return false;
        }

        public void Initialize(float speed, float distanceTreshold)
        {
            _speed = speed;
            _distanceTreshold = distanceTreshold;
        }

        public void Move(Vector3[] path, Action onStart, Action onComplete)
        {
            _waypointIndex = 0;
            _path = path;
            _destination = path[0];
            _isNeedToStop = false;
            _isMoving = true;
            _onStartMoving = onStart;
            _onCompleteMoving = onComplete;

            transform.DOLookAt(_destination, 0.1f, AxisConstraint.Y);

            _onStartMoving?.Invoke();
        }

        public void Stop()
        {
            _isNeedToStop = true;
        }

        private void ForceStop()
        {
            _isMoving = false;
        }
    }
}