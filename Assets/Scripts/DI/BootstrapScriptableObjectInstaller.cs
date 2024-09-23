using EchoOfTheTimes.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.DI
{
    [CreateAssetMenu(fileName = "Bootstrap Scriptable Object Installer",
        menuName = "ScriptableObjects/DI/BootstrapScriptableObjectInstaller", order = 3)]
    public class BootstrapScriptableObjectInstaller : ScriptableObjectInstaller<BootstrapScriptableObjectInstaller>
    {
        [SerializeField]
        private BootstrapSettingsScriptableObject _bootstrapSettings;

        public override void InstallBindings()
        {
            Container.Bind<BootstrapScriptableObjectInstaller>().FromScriptableObject(_bootstrapSettings).AsSingle();
        }
    }
}