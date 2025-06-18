using Systems.Units;

namespace Systems.Leveling
{
    public interface ISpecialVertex
    {
        public void OnEnter(IUnit unit);
        public void OnExit(IUnit unit);
    }
}