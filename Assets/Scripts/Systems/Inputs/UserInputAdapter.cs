using Systems.Leveling;
using Systems.Movement;
using Systems.Units;

namespace Systems.Inputs
{
    public class UserInputAdapter
    {
        private IUnit _unit;

        private readonly Input3DIndicator _3dIndicator;
        private readonly Input2DIndicator _2dIndicator;
        private readonly LevelStateMachine _stateMachine;

        public UserInputAdapter(LevelStateMachine stateMachine,
            Input3DIndicator input3DIndicator, Input2DIndicator input2DIndicator)
        {
            _stateMachine = stateMachine;

            _3dIndicator = input3DIndicator;
            _2dIndicator = input2DIndicator;
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
            _2dIndicator.ShowIndicator(to);

            if (_unit.TryMove(to))
            {
                _3dIndicator.ShowSuccessIndicator(to);
            }
            else
            {
                _3dIndicator.ShowErrorIndicator(to);
            }
        }

        public void SwitchState(int stateId)
        {
            if (_unit.CanInteractWithStates)
            {
                _stateMachine.ChangeState(stateId);
            }
        }
    }
}