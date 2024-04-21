using EchoOfTheTimes.UI;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.DI
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField]
        private UiLevelsGridService _uiLevelsGrid;

        public override void InstallBindings()
        {
            Container.Bind<UiLevelsGridService>().FromInstance(_uiLevelsGrid).AsSingle();
        }
    }
}