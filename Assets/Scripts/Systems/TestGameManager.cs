using System.Collections.Generic;
using Systems.Leveling;
using Systems.Movement;
using UnityEngine;

namespace Systems
{
    public class TestGameManager
    {
        public Vertex StartVertex;

        private Movable _movable;
        private GraphVisibility _graph;
        private List<Vertex> _verts;

        private List<Vertex> _path;

        private StateMachine _stateMachine;

        public TestGameManager()
        {
            _movable = Object.FindObjectOfType<Movable>();

            _movable.transform.position = StartVertex.transform.position;

            _graph = Object.FindObjectOfType<GraphVisibility>();
            _graph.ResetAndLoad();

            _verts = _graph.GetVertices();

            _movable.CurrentWaypoint = StartVertex;

            var stateables = new List<IStateable>();
            foreach (var item in Object.FindObjectsOfType<Stateable>())
            {
                stateables.Add(item);
            }
            _stateMachine = new StateMachine(3, stateables);
        }

        public void StopPlayer()
        {
            _movable.Stop();
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

        public void SwitchState(int stateId)
        {
            _stateMachine.ChangeState(stateId);
        }
    }
}