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
        private readonly RefinedOrbitCamera _orbitCamera;

        private Queue<IUnit> _units;
        private Queue<IUnit> _unitsBuffer;

        public UnitManagementService(UserInputAdapter inputAdapter, SpawnService spawnService, RefinedOrbitCamera orbitCamera)
        {
            _inputAdapter = inputAdapter;
            _spawnService = spawnService;
            _orbitCamera = orbitCamera;
        }

        public void Initialize()
        {
            _spawnService.RunSpawning();

            _units = new Queue<IUnit>(_spawnService.SpawnedUnits);
            _unitsBuffer = new Queue<IUnit>(_units);

            SwitchToNextUnit();
        }

        public void SwitchToNextUnit()
        {
            if (!_unitsBuffer.TryDequeue(out IUnit nextUnit))
            {
                _unitsBuffer = new Queue<IUnit>(_units);

                nextUnit = _unitsBuffer.Dequeue();
            }

            _inputAdapter.SetTarget(nextUnit);
            _orbitCamera.SetTarget(nextUnit);
        }
    }
}