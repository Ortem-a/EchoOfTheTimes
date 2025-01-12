using Systems.Leveling;
using Systems.Movement;

namespace Systems
{
    public class TestInputAdapter
    {
        private readonly Movable _movable;
        private readonly GraphVisibility _graph;
        private readonly StateMachine _stateMachine;

        public TestInputAdapter(Movable movable, GraphVisibility graph, StateMachine stateMachine)
        {
            _movable = movable;
            _graph = graph;
            _stateMachine = stateMachine;
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
            if (_movable.CurrentWaypoint.Id != to.Id)
            {
                Vertex start = _movable.NextWaypoint != null ? _movable.NextWaypoint : _movable.CurrentWaypoint;

                var path = _graph.GetPathBFS(start, to);
                _movable.MoveBy(path);
            }
        }

        public void SwitchState(int stateId)
        {
            if (_movable.OnBridge) return;
            
            _stateMachine.ChangeState(stateId);
        }
    }
}