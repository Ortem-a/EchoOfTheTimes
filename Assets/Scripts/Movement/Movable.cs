using DG.Tweening;
using EchoOfTheTimes.Core;
using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.ScriptableObjects.Player;
using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Movement
{
    public class Movable : MonoBehaviour
    {
        private PlayerPath _playerPath;

        private float _speed;
        private float _distanceTreshold;
        private float _rotateDuration;
        private AxisConstraint _rotateConstraint;

        private int _waypointIndex;
        private Vertex _destination;
        private Vertex[] _path;

        public Vertex Destination => _destination;

        private bool _isMoving = false;
        private bool _isNeedToStop = false;

        private Action _onStartMoving;
        private Action _onCompleteMoving;
        private Action _onStoppedMoving;

        [Inject]
        private void Construct(PlayerPath playerPath, PlayerSettingsScriptableObject playerSettings)
        {
            _playerPath = playerPath;

            _speed = playerSettings.MoveSpeed;
            _distanceTreshold = playerSettings.DistanceTreshold;
            _rotateDuration = playerSettings.RotateDuration;
            _rotateConstraint = playerSettings.AxisConstraint;
        }

        private void Update()
        {
            if (_isMoving)
            {
                if (Vector3.Distance(transform.position, _destination.transform.position) < _distanceTreshold)
                {
                    _onCompleteMoving?.Invoke();

                    if (_isNeedToStop)
                    {
                        _isNeedToStop = false;
                        ForceStop();
                        _playerPath.CompleteFullPath();

                        _onStoppedMoving?.Invoke();
                    }
                    else
                    {
                        if (!TryGetNextWaypoint(out _destination))
                        {
                            ForceStop();

                            _playerPath.CompleteFullPath();
                        }
                        else
                        {
#warning ÂÍÈÌÀÍÈÅ ÍÀ ÏÎÂÎÐÎÒÀÕ
                            transform.DOLookAt(_destination.transform.position, _rotateDuration, _rotateConstraint);

                            transform.position = Vector3.MoveTowards(transform.position, _destination.transform.position, _speed * Time.deltaTime);

                            _onStartMoving?.Invoke();
                        }
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, _destination.transform.position, _speed * Time.deltaTime);
                }
            }
        }

        public void Move(Vertex[] path, Action onStart, Action onComplete)
        {
            _waypointIndex = 0;
            _path = path;
            _destination = path[0];
            _isNeedToStop = false;
            _isMoving = true;
            _onStartMoving = onStart;
            _onCompleteMoving = onComplete;

            transform.DOLookAt(_destination.transform.position, _rotateDuration, _rotateConstraint);

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

        private bool TryGetNextWaypoint(out Vertex destination)
        {
            destination = null;
            _waypointIndex++;

            if (_waypointIndex < _path.Length)
            {
                _playerPath.CurrentVertexIndex = _waypointIndex;

                destination = _path[_waypointIndex];
                return true;
            }

            return false;
        }

        public void ChangePath(int leftVertices)
        {
            _path = _path.Skip(_waypointIndex).Take(leftVertices).ToArray();
        }

        public void ResetDestination()
        {
            ForceStop();
            _destination = null;
        }
    }
}