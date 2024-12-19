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

            _movable.CurrentWaypoint = StartVertex;

            SetPath(FinishVertex);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetPath(_verts[0]);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetPath(_verts[1]);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetPath(_verts[2]);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SetPath(_verts[3]);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SetPath(_verts[4]);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _movable.Stop();
            }
        }

        private void SetPath(Vertex to)
        {
            Vertex start = _movable.NextWaypoint != null ? _movable.NextWaypoint : _movable.CurrentWaypoint;

            _path = _graph.GetPathBFS(start, to);
            _movable.MoveBy(_path);
        }
    }
}