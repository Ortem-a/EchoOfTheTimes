using EchoOfTheTimes.ScriptableObjects.Level;
using EchoOfTheTimes.ScriptableObjects.Player;
using EchoOfTheTimes.ScriptableObjects.UI;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.DI
{
    [CreateAssetMenu(fileName = "Scene Scriptable Object Installer", menuName = "ScriptableObjects/DI/SceneScriptableObjectInstaller", order = 2)]
    public class SceneScriptableObjectsInstaller : ScriptableObjectInstaller<SceneScriptableObjectsInstaller>
    {
        [Header("Scene Scriptable Objects")]
        [SerializeField]
        private LevelSoundsSceneContainerScriptableObject _levelSoundsSceneContainer;
        [SerializeField]
        private SceneHudSoundsContainerScriptableObject _hudSoundsContainer;
        [SerializeField]
        private PlayerSoundsContainerScriptableObject _playerSoundsContainer;

        public override void InstallBindings()
        {
            Container.Bind<LevelSoundsSceneContainerScriptableObject>().FromScriptableObject(_levelSoundsSceneContainer).AsSingle();
            Container.Bind<SceneHudSoundsContainerScriptableObject>().FromScriptableObject(_hudSoundsContainer).AsSingle();
            Container.Bind<PlayerSoundsContainerScriptableObject>().FromScriptableObject(_playerSoundsContainer).AsSingle();
        }
    }
}