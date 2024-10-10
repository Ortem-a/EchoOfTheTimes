using EchoOfTheTimes.Persistence;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EchoOfTheTimes.UI.MainMenu
{
    public class HeadPanelSuperviser : MonoBehaviour
    {
        [SerializeField]
        private Button _aboutUsButton;
        [SerializeField]
        private Button _backToChaptersButton;
        [SerializeField]
        private MuteButtonController _muteSoundController;

        private bool _isSoundMuted;

        [Inject]
        private void Construct(UiMainMenuService mainMenuService)
        {
            _isSoundMuted = mainMenuService.PersistenceService.GetSettings();
        }

        private void Start()
        {
            _muteSoundController.SetButtonIcon(_isSoundMuted);
        }

        public void ShowHeadPanelForLevels() => ShowAboutUsButton(false);

        public void ShowHeadPanelForChapters() => ShowAboutUsButton(true);

        private void ShowAboutUsButton(bool show)
        {
            _aboutUsButton.gameObject.SetActive(show);
            _backToChaptersButton.gameObject.SetActive(!show);
        }
    }
}