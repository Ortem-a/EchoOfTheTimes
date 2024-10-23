using EchoOfTheTimes.Persistence;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EchoOfTheTimes.UI.MainMenu
{
    [RequireComponent(typeof(Image), typeof(Button))]
    public class MuteButtonController : MonoBehaviour
    {
        private Image _image;
        private Button _button;

        [SerializeField]
        private Sprite _mutedIcon;
        [SerializeField]
        private Sprite _soundEnableIcon;

        private PersistenceService _persistenceService;
        private bool _isMuted;

        [Inject]
        private void Construct(UiMainMenuService mainMenuService)
        {
            _persistenceService = mainMenuService.PersistenceService;
            _isMuted = _persistenceService.GetSettings();

            _image = GetComponent<Image>();

            _button = GetComponent<Button>();
            _button.onClick.AddListener(HandleButtonOnClick);
        }

        private void HandleButtonOnClick()
        {
            _isMuted = !_isMuted;

            _persistenceService.SetAndSaveSetttings(_isMuted);

            SetButtonIcon(_isMuted);
        }

        private void SetButtonIcon(bool isMuted)
        {
            if (isMuted)
            {
                _image.sprite = _mutedIcon;
            }
            else
            {
                _image.sprite = _soundEnableIcon;
            }
        }
    }
}