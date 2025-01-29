namespace Systems.Movement
{
    public interface ISpawnable
    {
        public IUnit Spawn(Vertex at);
    }
}