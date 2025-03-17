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
            InstallLeveling();

            InstallInputs();

            InstallMovement();

            InstallUnits();

            InstallCore();

            InstallUi();
        }

        private void InstallUi()
        {
            this.Container
                .BindInterfacesAndSelfTo<LevelStateUiButton>()
                .AsCached();

            this.Container
                .BindInterfacesAndSelfTo<SwitchUnitUiButton>()
                .AsCached();
        }

        private void InstallCore()
        {
            this.Container
                .Bind<GameLoopService>()
                .FromNew()
                .AsSingle();
        }

        private void InstallUnits()
        {
            this.Container
                .BindInterfacesAndSelfTo<UnitManagementService>()
                .AsSingle();
        }

        private void InstallInputs()
        {
            this.Container
                .Bind<UserInputAdapter>()
                .FromNew()
                .AsSingle();

            this.Container
                .Bind<TestUserInput>()
                .FromComponentInHierarchy()
                .AsSingle();

            this.Container
                .Bind<RefinedOrbitCamera>()
                .FromComponentInHierarchy()
                .AsSingle();

            this.Container
                .Bind<Input3DIndicator>()
                .FromComponentInHierarchy()
                .AsSingle();

            this.Container
                .Bind<Input2DIndicator>()
                .FromComponentInHierarchy()
                .AsSingle();
        }

        private void InstallMovement()
        {
            this.Container
                .Bind<GraphVisibility>()
                .FromComponentInHierarchy()
                .AsSingle();
        }

        private void InstallLeveling()
        {
            this.Container
                .BindInterfacesAndSelfTo<StateService>()
                .AsSingle();

            this.Container
                .Bind<LevelStateMachine>()
                .FromNew()
                .AsSingle();

            this.Container
                .Bind<SpawnService>()
                .FromComponentInHierarchy()
                .AsSingle();

            this.Container
                .BindInterfacesAndSelfTo<FinishButton>()
                .AsCached();

            this.Container
                .BindInterfacesAndSelfTo<LevelButton>()
                .AsCached();
        }
    }
}