using Systems.Units;

namespace Systems.Leveling
{
    public enum SpecialVertexType
    {
        Button,
        Teleportator,
    }

    public interface ISpecialVertex
    {
        public SpecialVertexType Type { get; }

        public void OnEnter(IUnit unit);
        public void OnExit(IUnit unit);
    }
}