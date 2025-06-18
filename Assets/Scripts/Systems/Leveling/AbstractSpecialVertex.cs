using Systems.Movement;
using Systems.Units;

namespace Systems.Leveling
{
    public abstract class AbstractSpecialVertex : VertexVisibility, ISpecialVertex
    {
        public abstract void OnEnter(IUnit unit);
        public abstract void OnExit(IUnit unit);
    }
}