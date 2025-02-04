using Systems.Core;
using Systems.Inputs;
using Systems.Leveling;
using Systems.Movement;
using Systems.UI.Level;
using Systems.Units;
using Zenject;

namespace Systems.DI
{
    public class TestLevelSceneInstaller : MonoInstaller
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
                .Bind<UserInputAdapter>()
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
                .BindInterfacesAndSelfTo<UnitManagementService>()
                .AsSingle();

            this.Container
                .Bind<GameLoopService>()
                .FromNew()
                .AsSingle();

            this.Container
                .BindInterfacesAndSelfTo<FinishButton>()
                .AsCached();

            this.Container
                .BindInterfacesAndSelfTo<LevelButton>()
                .AsCached();

            this.Container
                .BindInterfacesAndSelfTo<LevelStateUiButton>()
                .AsCached();

            this.Container
                .BindInterfacesAndSelfTo<SwitchUnitUiButton>()
                .AsCached();

            this.Container
                .Bind<RefinedOrbitCamera>()
                .FromComponentInHierarchy()
                .AsSingle();
        }
    }
}