using EchoOfTheTimes.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EchoOfTheTimes.UI.MainMenu
{
    [RequireComponent(typeof(Button))]
    public class LevelButtonHandler : MonoBehaviour
    {
        private Button _button;
        private SceneLoader _sceneLoader;
        private GameLevel _levelData;

        [Inject]
        private void Construct(UiMainMenuService mainMenuService)
        {
            _sceneLoader = mainMenuService.SceneLoader;
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(HandleButtonClicked);
        }

        public void SetData(GameLevel levelData) => _levelData = levelData;

        private async void HandleButtonClicked()
        {
            if (_levelData.LevelStatus == StatusType.Locked) return;

            await _sceneLoader.LoadSceneGroupAsync(_levelData);
        }
    }
}