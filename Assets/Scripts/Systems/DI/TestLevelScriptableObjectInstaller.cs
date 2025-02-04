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
        }
    }
}