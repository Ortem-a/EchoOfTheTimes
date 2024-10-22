using EchoOfTheTimes.Persistence;
using EchoOfTheTimes.SceneManagement;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.DI
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField]
        private SceneLoader _sceneLoader;
        [SerializeField]
        private PersistenceService _persistenceService;

        public override void InstallBindings()
        {
            Container.Bind<PersistenceService>().FromInstance(_persistenceService).AsSingle();
            Container.Bind<SceneLoader>().FromInstance(_sceneLoader).AsSingle();
        }
    }
}