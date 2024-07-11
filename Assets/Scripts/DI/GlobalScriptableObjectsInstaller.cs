using EchoOfTheTimes.Core;
using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Movement;
using EchoOfTheTimes.ScriptableObjects.Player;
using EchoOfTheTimes.ScriptableObjects.Level;
using EchoOfTheTimes.ScriptableObjects;
using EchoOfTheTimes.UI;
using EchoOfTheTimes.Units;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.DI
{
    [CreateAssetMenu(fileName = "Global Scriptable Object Installer", menuName = "ScriptableObjects/DI/GlobalScriptableObjectInstaller", order = 1)]
    public class GlobalScriptableObjectsInstaller : ScriptableObjectInstaller<GlobalScriptableObjectsInstaller>
    {
        [Header("Global Scriptable Objects")]
        [SerializeField]
        private ColorStateSettingsScriptableObject _colorStateSettings;
        [SerializeField]
        private PlayerSettingsScriptableObject _playerSettings;
        [SerializeField]
        private LevelSettingsScriptableObject _levelSettings;
        [SerializeField]
        private CameraSettingsScriptableObject _cameraSettings;
        [SerializeField]
        private InputIndicatorSettingsScriptableObject _inputIndicatorSettings;
        [SerializeField]
        private LevelSoundsGlobalContainerScriptableObject _levelSoundsGlobalContainer;

        public override void InstallBindings()
        {
            Container.Bind<PlayerSettingsScriptableObject>().FromScriptableObject(_playerSettings).AsSingle();
            Container.Bind<ColorStateSettingsScriptableObject>().FromScriptableObject(_colorStateSettings).AsSingle();
            Container.Bind<LevelSettingsScriptableObject>().FromScriptableObject(_levelSettings).AsSingle();
            Container.Bind<CameraSettingsScriptableObject>().FromScriptableObject(_cameraSettings).AsSingle();
            Container.Bind<InputIndicatorSettingsScriptableObject>().FromScriptableObject(_inputIndicatorSettings).AsSingle();
            Container.Bind<LevelSoundsGlobalContainerScriptableObject>().FromScriptableObject(_levelSoundsGlobalContainer).AsSingle();
        }
    }
}