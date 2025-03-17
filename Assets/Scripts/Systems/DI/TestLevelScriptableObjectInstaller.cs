using Systems.Settings;
using UnityEngine;
using Zenject;

namespace Systems.DI
{
    [CreateAssetMenu(fileName = "New Level Scriptable Object Installer", menuName = "Installers/Level Scriptable Object Installer")]
    public class TestLevelScriptableObjectInstaller : ScriptableObjectInstaller<TestLevelScriptableObjectInstaller>
    {
        [SerializeField]
        private CameraSettingsScriptableObject _cameraSettings;
        [SerializeField]
        private InputIndicatorSettingsScriptableObject _inputIndicatorSettings;

        [Header("SFX")]
        [SerializeField]
        private LevelSoundsContainerScriptableObject _levelSoundsContainer;
        [SerializeField]
        private UnitSoundsContainerScriptableObject _unitSoundsContainer;

        public override void InstallBindings()
        {
            this.Container
                .Bind<CameraSettingsScriptableObject>()
                .FromScriptableObject(_cameraSettings)
                .AsSingle();

            this.Container
                .Bind<InputIndicatorSettingsScriptableObject>()
                .FromScriptableObject(_inputIndicatorSettings)
                .AsSingle();

            this.Container
                .Bind<LevelSoundsContainerScriptableObject>()
                .FromScriptableObject(_levelSoundsContainer)
                .AsSingle();

            this.Container
                .Bind<UnitSoundsContainerScriptableObject>()
                .FromScriptableObject(_unitSoundsContainer)
                .AsSingle();
        }
    }
}