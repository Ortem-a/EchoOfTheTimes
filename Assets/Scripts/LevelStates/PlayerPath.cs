using EchoOfTheTimes.Core;
using EchoOfTheTimes.Movement;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    public class PlayerPath : MonoBehaviour
    {
        private enum PathType
        {
            Static,
            Dynamic
        }

        public List<Vertex> Path;

        private bool _isMarkNeeded = false;

        [SerializeField]
        private PathType _pathType;

        private List<PathType> _pathTypes;

        public int CurrentVertexIndex = 0;
        public int FirstDynamicIndex;
        public int LastDynamicIndex;

        public Movable Movable;

        public bool StayOnDynamic => Path != null && Path.Count > 0 ? Path[CurrentVertexIndex].IsDynamic : false;

        public void SetPath(List<Vertex> path)
        {
            Path = path;

            GetPathType(path, out FirstDynamicIndex, out LastDynamicIndex);

            CurrentVertexIndex = 0;
            _isMarkNeeded = true;
        }

        private void OnDrawGizmos()
        {
            if (_isMarkNeeded)
            {
                for (int i = 0; i < Path.Count; i++)
                {
                    if (Path[i].IsDynamic)
                        Gizmos.color = Color.blue;
                    else
                        Gizmos.color = Color.green;

                    Gizmos.DrawCube(Path[i].transform.position, Vector3.one * 0.35f);
                }

                Gizmos.color = Color.magenta;

                Gizmos.DrawCube(Path[CurrentVertexIndex].transform.position, Vector3.one * 0.35f);

                Gizmos.color = Color.red;
                for (int i = CurrentVertexIndex; i < FirstDynamicIndex - CurrentVertexIndex; i++)
                {
                    Gizmos.DrawCube(Path[i].transform.position + Vector3.up / 2f, Vector3.one * 0.35f);
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
                    if (CurrentVertexIndex < FirstDynamicIndex)
                    {
                        Movable.ChangePath(FirstDynamicIndex - CurrentVertexIndex);
                    }
                    else if (CurrentVertexIndex > LastDynamicIndex)
                    {
                        _pathType = PathType.Static;
                    }
                }
            }

            //if (_pathType == PathType.Dynamic)
            //{
            //    Movable.ChangePath(FirstDynamicIndex - CurrentVertexIndex);
            //}
        }
    }
}