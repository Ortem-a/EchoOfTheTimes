using System.Collections.Generic;
using Systems.Inputs;
using Systems.Leveling;
using Zenject;

namespace Systems.Units
{
    public class UnitManagementService : IInitializable
    {
        private readonly UserInputAdapter _inputAdapter;
        private readonly SpawnService _spawnService;

        private Queue<IUnit> _units;
        private Queue<IUnit> _unitsBuffer;

        public UnitManagementService(UserInputAdapter inputAdapter, SpawnService spawnService)
        {
            _inputAdapter = inputAdapter;
            _spawnService = spawnService;
        }

        public void Initialize()
        {
            var firstSpawnedUnit = _spawnService.RunSpawning();
            _inputAdapter.SetTarget(firstSpawnedUnit);

            _units = new Queue<IUnit>(_spawnService.SpawnedUnits);
            _unitsBuffer = new Queue<IUnit>(_units);
        }

        public void SwitchToNextUnit()
        {
            if (!_unitsBuffer.TryDequeue(out IUnit nextUnit))
            {
                _unitsBuffer = new Queue<IUnit>(_units);

                nextUnit = _unitsBuffer.Dequeue();
            }

            _inputAdapter.SetTarget(nextUnit);
        }
    }
}