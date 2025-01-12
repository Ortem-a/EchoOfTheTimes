using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Movement
{
    public class PathService : MonoBehaviour
    {
        private Movable _target;

        private Queue<Vertex> _bufferPath;
        private Queue<Vertex> _path;

        public Action OnStateChanged;

        private void Awake()
        {
            OnStateChanged += CheckPath;
        }

        private void OnDestroy()
        {
            OnStateChanged -= CheckPath;
        }

        private void CheckPath()
        {
            // посмотреть оставшийся путь
            // если надо, то обрезать путь

            var currentParent = _target.GetMarkerParent();

            
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