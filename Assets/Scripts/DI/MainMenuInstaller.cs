using EchoOfTheTimes.UI;
using EchoOfTheTimes.UI.MainMenu;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.DI
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField]
        private UiLevelsGridService _uiLevelsGrid;
        [SerializeField]
        private ColumnService _columnService;

        public override void InstallBindings()
        {
            Container.Bind<UiLevelsGridService>().FromInstance(_uiLevelsGrid).AsSingle();
            Container.Bind<ColumnService>().FromInstance(_columnService).AsSingle();
        }
    }
}