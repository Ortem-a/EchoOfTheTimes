using EchoOfTheTimes.Core;
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

        public int CurrentVertexIndex;
        public int FirstDynamicIndex;

        public void SetPath(List<Vertex> path)
        {
            Path = path;

            _pathType = GetPathType(path, out FirstDynamicIndex);

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
                        Gizmos.color = Color.green;
                    else
                        Gizmos.color = Color.magenta;

                    Gizmos.DrawCube(Path[i].transform.position + Vector3.up / 2f, Vector3.one * 0.25f);
                }

                Gizmos.color = Color.blue;

                Gizmos.DrawCube(Path[CurrentVertexIndex].transform.position + Vector3.up / 2f, Vector3.one * 0.25f);

                Gizmos.color = Color.white;
            }
        }

        private PathType GetPathType(List<Vertex> path, out int firstDynamicIndex)
        {
            firstDynamicIndex = -1;

            for (int i = 0; i < path.Count; i++)
            {
                if (path[i].IsDynamic)
                {
                    firstDynamicIndex = i;
                    return PathType.Dynamic;
                }
            }

            return PathType.Static;
        }
    }
}