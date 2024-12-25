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
        }

        private void Start()
        {
            SetPath(FinishVertex);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _movable.Stop();
            }
        }

        public void HandleTouch(Vertex to)
        {
            SetPath(to);
        }

        private void SetPath(Vertex to)
        {
            Vertex start = _movable.NextWaypoint != null ? _movable.NextWaypoint : _movable.CurrentWaypoint;

            _path = _graph.GetPathBFS(start, to);
            _movable.MoveBy(_path);
        }
    }
}