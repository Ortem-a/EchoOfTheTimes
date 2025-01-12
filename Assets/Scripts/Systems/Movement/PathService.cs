using System;
using System.Collections.Generic;
using Systems.Leveling;
using UnityEngine;
using Zenject;

namespace Systems.Movement
{
    public class PathService : MonoBehaviour
    {
        private Movable _target;

        private Queue<Vertex> _bufferPath;
        private Queue<Vertex> _path;

        public Action OnStateChanged;

        private StateService _stateService;
        private TestInputAdapter _testInputAdapter;

        [Inject]
        private void Construct(StateService stateService, TestInputAdapter testInputAdapter)
        {
            _stateService = stateService;
            _testInputAdapter = testInputAdapter;

            _stateService.OnStartChangingState += CheckPath;
            //OnStateChanged += CheckPath;
        }

        private void OnDestroy()
        {
            _stateService.OnStartChangingState -= CheckPath;
            //OnStateChanged -= CheckPath;
        }

        private void CheckPath()
        {
            // перестроить путь в новом графе
            
        }

        public void SetPath(List<Vertex> path)
        {
            if (path.Count != 0)
            {
                path.Reverse();
                _bufferPath = new Queue<Vertex>(path);

                if (_target.IsMoving)
                {
                    // Stop();
                }
                else
                {
                    _path = new Queue<Vertex>(_bufferPath);
                    //NextWaypoint = _bufferPath.Dequeue();

                    _bufferPath.Clear();

                    // Move()
                }
            }
        }
    }
}