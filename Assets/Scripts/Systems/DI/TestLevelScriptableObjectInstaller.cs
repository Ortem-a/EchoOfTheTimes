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

        public override void InstallBindings()
        {
            this.Container
                .Bind<CameraSettingsScriptableObject>()
                .FromScriptableObject(_cameraSettings)
                .AsSingle();
        }
    }
}