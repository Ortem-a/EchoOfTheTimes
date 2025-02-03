using Systems.Leveling;
using Systems.Movement;
using Systems.Units;

namespace Systems.Inputs
{
    public class TestInputAdapter
    {
        private IUnit _unit;

        private readonly LevelStateMachine _stateMachine;

        public TestInputAdapter(LevelStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void SetTarget(IUnit unit)
        {
            _unit = unit;
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