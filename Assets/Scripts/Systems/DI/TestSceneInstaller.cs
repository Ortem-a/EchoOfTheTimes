using Systems.Core;
using Systems.Inputs;
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
                .Bind<LevelStateMachine>()
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

            this.Container
                .Bind<GameLoopService>()
                .FromNew()
                .AsSingle();

            this.Container
                .BindInterfacesAndSelfTo<FinishButton>()
                .AsCached();
        }
    }
}