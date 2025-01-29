using Systems.Movement;
using UnityEngine;
using Zenject;

namespace Systems.Leveling
{
    public class UnitFactory : IFactory<GameObject, IUnit>
    {
        private readonly GraphVisibility _graph;
        private readonly DiContainer _container;

        public UnitFactory(DiContainer container, GraphVisibility graph)
        {
            _container = container;
            _graph = graph;
        }

        public IUnit Create(GameObject prefab)
        {
            var unit = _container.InstantiatePrefabForComponent<IUnit>(prefab);
            return unit;
        }
    }
}