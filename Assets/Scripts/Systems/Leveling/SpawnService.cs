using System.Collections.Generic;
using Systems.Movement;
using Systems.Settings;
using Systems.Units;
using UnityEngine;
using Zenject;

namespace Systems.Leveling
{
    [System.Serializable]
    public class Spawnable
    {
        public UnitSettingsScriptableObject UnitSettings;
        public Vertex At;
    }

    public class SpawnService : MonoBehaviour
    {
        private IUnit.Factory _factory;
        private GraphVisibility _graph;

        [SerializeField]
        private List<Spawnable> _spawnables;
        public List<IUnit> SpawnedUnits = new List<IUnit>();

        [Inject]
        private void Construct(GraphVisibility graph)
        {
            _graph = graph;

            _factory = new IUnit.Factory();
        }

        public void RunSpawning()
        {
#warning NEED TO ASYNC SPAWN
            for (int i = 0; i < _spawnables.Count; i++)
            {
                var unit = _factory.Create(_spawnables[i].UnitSettings, _spawnables[i].At, _graph);
                SpawnedUnits.Add(unit);
            }
        }
    }
}