using EchoOfTheTimes.Core;
using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Movement;
using EchoOfTheTimes.ScriptableObjects;
using EchoOfTheTimes.UI;
using EchoOfTheTimes.Units;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.DI
{
    [CreateAssetMenu(fileName = "Installer settings", menuName = "ScriptableObjects/DI")]
    public class ScriptableObjectsInstaller : ScriptableObjectInstaller<ScriptableObjectsInstaller>
    {
        [Header("Scriptable Objects")]
        [SerializeField]
        private ColorStateSettingsScriptableObject _colorStateSettings;
        [SerializeField]
        private PlayerSettingsScriptableObject _playerSettings;
        [SerializeField]
        private LevelSettingsScriptableObject _levelSettings;
        [SerializeField]
        private CameraSettingsScriptableObject _cameraSettings;

        public override void InstallBindings()
        {
            Container.Bind<PlayerSettingsScriptableObject>().FromScriptableObject(_playerSettings).AsSingle();
            Container.Bind<ColorStateSettingsScriptableObject>().FromScriptableObject(_colorStateSettings).AsSingle();
            Container.Bind<LevelSettingsScriptableObject>().FromScriptableObject(_levelSettings).AsSingle();
            Container.Bind<CameraSettingsScriptableObject>().FromScriptableObject(_cameraSettings).AsSingle();
        }
    }
}