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
        private Player _player;
        [SerializeField]
        private GraphVisibility _graph;
        [SerializeField]
        private StateService _stateService;

        public override void InstallBindings()
        {
            Container.Bind<TestUserInput>().FromInstance(_userInput).AsSingle();

            Container.Bind<IUnit>().FromInstance(_player).AsSingle();
            Container.Bind<GraphVisibility>().FromInstance(_graph).AsSingle();
            Container.Bind<TestInputAdapter>().FromNew().AsSingle();
            Container.Bind<StateMachine>().FromNew().AsSingle();
            Container.Bind<StateService>().FromInstance(_stateService).AsSingle();

            Container.Bind<EntryPointService>().FromInstance(_entryPointService).AsSingle().NonLazy();
        }
    }
}