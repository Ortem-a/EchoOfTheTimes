using Systems.Movement;

namespace Systems.Units
{
    public interface ISpawnable
    {
        public IUnit Spawn(Vertex at, GraphVisibility graph);
    }
}