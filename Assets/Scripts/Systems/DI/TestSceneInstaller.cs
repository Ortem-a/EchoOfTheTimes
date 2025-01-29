using Systems.Leveling;
using Systems.Movement;
using UnityEngine;
using Zenject;

namespace Systems.DI
{
    public class TestSceneInstaller : MonoInstaller
    {
        [SerializeField]
        private EntryPointService _entryPointService;
        [SerializeField]
        private TestUserInput _userInput;
        [SerializeField]
        private GraphVisibility _graph;
        [SerializeField]
        private StateService _stateService;

        [SerializeField]
        private Spawner _spawner;

        public override void InstallBindings()
        {
            Container.Bind<StateService>().FromInstance(_stateService).AsSingle();

            //Container.Bind<StateMachine>().FromNew().AsSingle();

            //Container.Bind<TestUserInput>().FromInstance(_userInput).AsSingle();

            Container.Bind<GraphVisibility>().FromInstance(_graph).AsSingle();

            //Container.Bind<TestInputAdapter>().FromNew().AsSingle();

            Container.Bind<EntryPointService>().FromInstance(_entryPointService).AsSingle().NonLazy();

            Container.Bind<Spawner>().FromInstance(_spawner).AsSingle();
        }
    }
}