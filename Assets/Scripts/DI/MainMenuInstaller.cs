using EchoOfTheTimes.UI.MainMenu;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.DI
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField]
        private UiMainMenuService _uiMainMenuService;

        public override void InstallBindings()
        {
            Container.Bind<UiMainMenuService>().FromInstance(_uiMainMenuService).AsSingle();
        }
    }
}