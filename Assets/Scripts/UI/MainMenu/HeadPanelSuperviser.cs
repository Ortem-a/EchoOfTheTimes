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

        [Inject]
        private void Construct(UiMainMenuService mainMenuService)
        {
            _backToChaptersButton.onClick.AddListener(mainMenuService.ShowChaptersList);
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