using Systems.Movement;
using Systems.Settings;
using UnityEngine;

namespace Systems.Units
{
    public interface IUnit : ISpawnable
    {
        public IMovableByPath Movable { get; }
        public ITeleportable Teleportable { get; }
        public Transform Transform { get; }
        public bool CanInteractWithStates { get; set; }

        public bool TryMove(Vertex to);

        public class Factory
        {
            public IUnit Create(UnitSettingsScriptableObject unitSettings, Vertex at, GraphVisibility graph)
            {
                var obj = Object.Instantiate(unitSettings.Prefab);

                var unit = obj.GetComponent<IUnit>();

                return unit.Spawn(unitSettings, at, graph);
            }
        }
    }
}