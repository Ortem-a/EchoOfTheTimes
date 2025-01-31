using Systems.Leveling;
using Systems.Movement;
using Zenject;

namespace Systems.DI
{
    public class TestSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container
                .BindInterfacesAndSelfTo<StateService>()
                .AsSingle();

            this.Container
                .Bind<StateMachine>()
                .FromNew()
                .AsSingle();

            this.Container
                .Bind<TestInputAdapter>()
                .FromNew()
                .AsSingle();

            this.Container
                .Bind<TestUserInput>()
                .FromComponentInHierarchy()
                .AsSingle();

            this.Container
                .Bind<GraphVisibility>()
                .FromComponentInHierarchy()
                .AsSingle();

            this.Container
                .Bind<SpawnService>()
                .FromComponentInHierarchy()
                .AsSingle();

            this.Container
                .BindInterfacesAndSelfTo<EntryPointService>()
                .AsSingle();
        }
    }
}