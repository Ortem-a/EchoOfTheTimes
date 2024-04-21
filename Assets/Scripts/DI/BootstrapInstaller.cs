using EchoOfTheTimes.SceneManagement;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.DI
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField]
        private SceneLoader _sceneLoader;

        public override void InstallBindings()
        {
            Container.Bind<SceneLoader>().FromInstance(_sceneLoader).AsSingle();
        }
    }
}