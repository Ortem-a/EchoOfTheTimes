using EchoOfTheTimes.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EchoOfTheTimes.UI
{
    public class UiStateButton : MonoBehaviour
    {
        private InputMediator _inputMediator;
        private UiSceneController _uiSceneController;
        private Button _button;
        private Image _image;

        private Color _deselectedColor;
        private Color _selectedColor;

        public void Init(int stateId, InputMediator inputHandler, UiSceneController uiSceneController, Color deselectedColor, Color selectedColor)
        {
            _inputMediator = inputHandler;
            _uiSceneController = uiSceneController;

            _deselectedColor = deselectedColor;
            _selectedColor = selectedColor;

            _button = GetComponent<Button>();
            _image = GetComponent<Image>();

            _button.transform.GetChild(0).GetComponent<TMP_Text>().text = $"To {stateId}";
            _button.onClick.AddListener(() => ChangeState(stateId));
        }

        private void ChangeState(int stateId)
        {
            Select();
            _uiSceneController.DeselectAllButtons(stateId);

            _inputMediator.ChangeLevelState(stateId);
        }

        public void Deselect() => _image.color = _deselectedColor;

        public void Select() => _image.color = _selectedColor;
    }
}