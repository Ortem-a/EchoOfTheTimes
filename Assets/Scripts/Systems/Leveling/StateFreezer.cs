using Systems.Units;

namespace Systems.Leveling
{
    public sealed class StateFreezer : AbstractSpecialVertex
    {
        public override void OnEnter(IUnit unit)
        {
            unit.CanInteractWithStates = false;
        }

        public override void OnExit(IUnit unit)
        {
            unit.CanInteractWithStates = true;
        }
    }
}
