using Systems.Leveling;
using Systems.Movement;

namespace Systems
{
    public class TestInputAdapter
    {
        private readonly IUnit _unit;

        private readonly StateMachine _stateMachine;

        public TestInputAdapter(IUnit movable, StateMachine stateMachine)
        {
            _unit = movable;
            _stateMachine = stateMachine;
        }

        public void StopPlayer()
        {
            _unit.Movable.Stop();
        }

        public void HandleTouch(Vertex to)
        {
            _unit.Move(to);
        }

        public void SwitchState(int stateId)
        {
            if (_unit.Movable.OnBridge) return;
            
            _stateMachine.ChangeState(stateId);
        }
    }
}