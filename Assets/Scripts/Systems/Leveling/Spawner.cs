using Systems.Movement;
using UnityEngine;
using Zenject;

namespace Systems.Leveling
{
    public class Spawner : IInitializable
    {
        private IUnit.Factory _factory;
        private TestInputAdapter _inputAdapter;
        private TestUserInput _userInput;

        public GameObject Prefab;
        public Vertex At;

        public Spawner(IUnit.Factory factory, TestUserInput userInput)
        {
            _factory = factory;
            _userInput = userInput;

        }

        public void Initialize()
        {
            var unit = _factory.Create(Prefab);
            unit.Spawn(At);

            //_inputAdapter = new TestInputAdapter(unit, _stateMachine);
            //_userInput.SetAdapter(_inputAdapter);
        }
    }
}