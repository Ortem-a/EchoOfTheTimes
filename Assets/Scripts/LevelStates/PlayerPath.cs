using EchoOfTheTimes.Core;
using EchoOfTheTimes.Movement;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.LevelStates
{
    public class PlayerPath : MonoBehaviour
    {
        private enum PathType
        {
            Static,
            Dynamic
        }

        private List<Vertex> _path;

        private bool _isMarkNeeded = false;

        private PathType _pathType;
        private List<PathType> _pathTypes;

        [HideInInspector]
        public int CurrentVertexIndex = 0;
        private int _firstDynamicIndex;
        private int _lastDynamicIndex;

        private Movable _movable;

        public bool StayOnDynamic => _path != null && _path.Count > 0 ? _path[CurrentVertexIndex].IsDynamic : false;
        public bool PrevIsDynamic => _path != null && _path.Count > 1 && CurrentVertexIndex > 0 ? _path[CurrentVertexIndex - 1].IsDynamic : false;

        [Inject]
        private void Construct(Movable movable)
        {
            _movable = movable;
        }

        public void SetPath(List<Vertex> path)
        {
            _path = path;

            GetPathType(path, out _firstDynamicIndex, out _lastDynamicIndex);

            CurrentVertexIndex = 0;
            _isMarkNeeded = true;
        }

        private void OnDrawGizmos()
        {
            if (_isMarkNeeded)
            {
                for (int i = 0; i < _path.Count; i++)
                {
                    if (_path[i].IsDynamic)
                        Gizmos.color = Color.blue;
                    else
                        Gizmos.color = Color.green;

                    Gizmos.DrawCube(_path[i].transform.position, Vector3.one * 0.35f);
                }

                Gizmos.color = Color.magenta;

                Gizmos.DrawCube(_path[CurrentVertexIndex].transform.position, Vector3.one * 0.35f);

                Gizmos.color = Color.red;
                for (int i = CurrentVertexIndex; i < _firstDynamicIndex - CurrentVertexIndex; i++)
                {
                    Gizmos.DrawCube(_path[i].transform.position + Vector3.up / 2f, Vector3.one * 0.35f);
                }

                Gizmos.color = Color.white;
            }
        }

        private void GetPathType(List<Vertex> path, out int firstDynamicIndex, out int lastDynamicIndex)
        {
            firstDynamicIndex = -1;
            lastDynamicIndex = -1;

            _pathType = PathType.Static;
            _pathTypes = new List<PathType>();

            for (int i = 0; i < path.Count; i++)
            {
                if (path[i].IsDynamic)
                {
                    _pathTypes.Add(PathType.Dynamic);
                    _pathType = PathType.Dynamic;
                }
                else
                {
                    _pathTypes.Add(PathType.Static);
                }
            }

            if (_pathType == PathType.Dynamic)
            {
                bool isFirstTime = true;

                for (int i = 0; i < _pathTypes.Count; i++)
                {
                    if (_pathTypes[i] == PathType.Dynamic)
                    {
                        if (isFirstTime)
                        {
                            firstDynamicIndex = i;
                            isFirstTime = false;
                        }
                        lastDynamicIndex = i;
                    }
                }
            }
        }

        public void CutPath()
        {
            if (_pathType == PathType.Dynamic)
            {
                if (_pathTypes[CurrentVertexIndex] == PathType.Static)
                {
                    // before dynamic part
                    if (CurrentVertexIndex < _firstDynamicIndex)
                    {
                        _movable.ChangePath(_firstDynamicIndex - CurrentVertexIndex);
                    }
                    // after dynamic part
                    else if (CurrentVertexIndex > _lastDynamicIndex)
                    {
                        _pathType = PathType.Static;
                    }
                }
            }
        }
    }
}