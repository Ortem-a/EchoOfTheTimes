using Systems.Inputs;
using Systems.Leveling;
using Zenject;

namespace Systems.Core
{
    public class EntryPointService : IInitializable
    {
        private readonly SpawnService _spawner;
        private readonly TestInputAdapter _inputAdapter;

        public EntryPointService(SpawnService spawner, TestInputAdapter inputAdapter)
        {
            _spawner = spawner;
            _inputAdapter = inputAdapter;
        }

        public void Initialize()
        {
            var player = _spawner.RunSpawner();
            _inputAdapter.SetTarget(player);
        }
    }
}