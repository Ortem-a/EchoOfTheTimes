using System.Collections.Generic;
using Systems.Movement;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

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
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                HandleInputToVertex((int)KeyCode.Alpha1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                HandleInputToVertex((int)KeyCode.Alpha2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                HandleInputToVertex((int)KeyCode.Alpha3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                HandleInputToVertex((int)KeyCode.Alpha4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                HandleInputToVertex((int)KeyCode.Alpha5);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                HandleInputToVertex((int)KeyCode.Alpha6);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _movable.Stop();
            }
        }

        private void HandleInputToVertex(int keyCode)
        {
            int vertexIndex = keyCode - (int)KeyCode.Alpha0 - 1;
            SetPath(_verts[vertexIndex]);
        }

        private void SetPath(Vertex to)
        {
            Vertex start = _movable.NextWaypoint != null ? _movable.NextWaypoint : _movable.CurrentWaypoint;

            _path = _graph.GetPathBFS(start, to);
            _movable.MoveBy(_path);
        }
    }
}