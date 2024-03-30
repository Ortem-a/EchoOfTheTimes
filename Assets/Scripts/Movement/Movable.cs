using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Animations;

namespace EchoOfTheTimes.Movement
{
    public class Movable : MonoBehaviour
    {
        private float _speed;
        private float _distanceTreshold;
        private float _rotateDuration;
        private AxisConstraint _rotateConstraint;

        private int _waypointIndex;
        private Vector3 _destination;
        private Vector3[] _path;

        private bool _isMoving = false;
        private bool _isNeedToStop = false;

        private Action _onStartMoving;
        private Action _onCompleteMoving;
        private Action _onStoppedMoving;

        private void Update()
        {
            if (_isMoving) 
            {
                if (Vector3.Distance(transform.position, _destination) < _distanceTreshold)
                {
                    _onCompleteMoving?.Invoke();

                    if (_isNeedToStop)
                    {
                        _isNeedToStop = false;
                        ForceStop();
                        _onStoppedMoving?.Invoke();
                    }
                    else
                    {
                        if (!TryGetNextWaypoint(out _destination))
                        {
                            ForceStop();
                        }
                        else
                        {
                            transform.DOLookAt(_destination, _rotateDuration, _rotateConstraint);

                            _onStartMoving?.Invoke();
                        }
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, _destination, _speed * Time.deltaTime);
                }
            }
        }

        public void Initialize(float speed, float distanceTreshold, float rotateDuration, AxisConstraint rotateConstraint)
        {
            _speed = speed;
            _distanceTreshold = distanceTreshold;
            _rotateDuration = rotateDuration;
            _rotateConstraint = rotateConstraint;
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

            transform.DOLookAt(_destination, _rotateDuration, _rotateConstraint);

            _onStartMoving?.Invoke();
        }

        public void Stop(Action onStopped)
        {
            _isNeedToStop = true;
            _onStoppedMoving = onStopped;

            if (!_isMoving)
            {
                ForceStop();
                _onStoppedMoving?.Invoke();
            }
        }

        private void ForceStop()
        {
            _isMoving = false;
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
    }
}