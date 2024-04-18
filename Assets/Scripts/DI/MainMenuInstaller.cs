using EchoOfTheTimes.UI;
using UnityEngine;
using Zenject;

public class MainMenuInstaller : MonoInstaller
{
    [SerializeField]
    private UiLevelsController _uiLevelsController;

    public override void InstallBindings()
    {
        //Container.Bind<UiLevelsController>().FromInstance(_uiLevelsController).AsSingle();
    }
}
