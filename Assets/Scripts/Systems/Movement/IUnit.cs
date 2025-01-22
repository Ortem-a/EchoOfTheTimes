namespace Systems.Movement
{
    public interface IUnit : ISpawnable
    {
        public IMovableByPath Movable { get; }
        public ITeleportable Teleportable { get; }
        public void Move(Vertex to);
    }
}