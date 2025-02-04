using Systems.Movement;
using UnityEngine;

namespace Systems.Units
{
    public interface IUnit : ISpawnable
    {
        public IMovableByPath Movable { get; }
        public ITeleportable Teleportable { get; }
        public Transform Transform { get; }

        public void Move(Vertex to);

        public class Factory
        {
            public IUnit Create(GameObject prefab, Vertex at, GraphVisibility graph)
            {
                var obj = MonoBehaviour.Instantiate(prefab);

                var unit = obj.GetComponent<IUnit>();

                return unit.Spawn(at, graph);
            }
        }
    }
}