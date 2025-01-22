namespace Systems.Movement
{
    public interface IUnit
    {
        public IMovableByPath Movable { get; }
        public ITeleportable Teleportable { get; }
    }
}