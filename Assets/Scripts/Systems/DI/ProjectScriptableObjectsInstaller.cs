using Systems.Settings;
using UnityEngine;
using Zenject;

namespace Systems.DI
{
    [CreateAssetMenu(
        fileName = "New Project Scriptable Object Installer", 
        menuName = "Installers/Project Scriptable Object Installer",
        order = 6)]
    public class ProjectScriptableObjectsInstaller : ScriptableObjectInstaller<ProjectScriptableObjectsInstaller>
    {
        [Header("Global Scriptable Objects")]
        [SerializeField]
        private InputIndicatorSettingsScriptableObject _inputIndicatorSettings;

        [SerializeField]
        private LevelSettingsScriptableObject _levelSettings;

        public override void InstallBindings()
        {
            this.Container
                .Bind<InputIndicatorSettingsScriptableObject>()
                .FromScriptableObject(_inputIndicatorSettings)
                .AsSingle();

            this.Container
                .Bind<LevelSettingsScriptableObject>()
                .FromScriptableObject(_levelSettings)
                .AsSingle();
        }
    }
}