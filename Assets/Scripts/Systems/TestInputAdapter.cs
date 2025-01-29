using Systems.Leveling;
using Systems.Movement;

namespace Systems
{
    public class TestInputAdapter
    {
        private readonly IUnit _unit;

        private readonly StateMachine _stateMachine;

        public TestInputAdapter(IUnit unit, StateMachine stateMachine)
        {
            _unit = unit;
            _stateMachine = stateMachine;
        }

        public void StopUnit()
        {
            _unit.Movable.Stop();
        }

        public void HandleTouch(Vertex to)
        {
            _unit.Move(to);
        }

        public void SwitchState(int stateId)
        {
            _stateMachine.ChangeState(stateId);
        }
    }
}