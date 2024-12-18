using System.Collections.Generic;
using Systems.Movement;
using UnityEngine;

namespace Systems
{
    public class TestGameManager : MonoBehaviour
    {
        public Vertex StartVertex;
        public Vertex FinishVertex;

        private Movable _movable;
        private GraphVisibility _graph;
        private List<Vertex> _verts; 

        private List<Vertex> _path;

        private void Awake()
        {
            _movable = FindObjectOfType<Movable>();

            _movable.transform.position = StartVertex.transform.position;

            _graph = FindObjectOfType<GraphVisibility>();
            _graph.ResetAndLoad();

            _verts = _graph.GetVertices();

            _movable.NextWaypoint = StartVertex;

            SetPath(StartVertex, FinishVertex);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetPath(_movable.NextWaypoint, _verts[0]);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetPath(_movable.NextWaypoint, _verts[1]);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetPath(_movable.NextWaypoint, _verts[2]);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SetPath(_movable.NextWaypoint, _verts[3]);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SetPath(_movable.NextWaypoint, _verts[4]);
            }
        }

        private void SetPath(Vertex start, Vertex finish)
        {
            _path = _graph.GetPathBFS(start, finish);
            _movable.MoveBy(_path);
        }

        private void OnDrawGizmos()
        {
            if (_path != null)
            {
                Gizmos.color = Color.blue;

                foreach (Vertex v in _path)
                {
                    Gizmos.DrawSphere(v.transform.position, .15f);
                }
            }
        }
    }
}