using EchoOfTheTimes.Core;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace EchoOfTheTimes.UI
{
    [RequireComponent(typeof(Button), typeof(Animator))]
    public class UiStateButton : MonoBehaviour
    {
        private InputMediator _inputMediator;
        private UiSceneController _uiSceneController;
        private HUDController _hudController;
        private Button _button;
        private Animator _animator;
        private bool _isSelected;
        private float _spawnTime;

        private Image[] _shadows;

        public void Init(int stateId, InputMediator inputHandler, UiSceneController uiSceneController,
            HUDController hudController, RuntimeAnimatorController animatorController)
        {
            _inputMediator = inputHandler;
            _uiSceneController = uiSceneController;
            _hudController = hudController;

            _button = GetComponent<Button>();
            _button.transition = Selectable.Transition.None;
            _button.onClick.AddListener(() => ChangeState(stateId));

            _animator = GetComponent<Animator>();
            _animator.runtimeAnimatorController = animatorController;

            _shadows = GetComponentsInChildren<Image>().Where(image => image.gameObject.name.Contains("Shadow")).ToArray();
            SetShadowColor(_uiSceneController.DefaultStateButtonColor);

            _hudController.RegisterButton(this);

            _spawnTime = Time.time;  // Записываем время спавна кнопки
        }

        private void ChangeState(int stateId)
        {
            if (Time.time - _spawnTime < 2.9f) return; // Проверяем, прошло ли 1 секунда после спавна -- ЭТО КОСТЫЛЬ

            Select();
            _uiSceneController.DeselectAllButtons(stateId);
            _inputMediator.ChangeLevelState(stateId);
        }

        public void Deselect() => _animator.SetBool("Tap", false);

        public void Select() => _animator.SetBool("Tap", true);

        public void SetInteractable(bool isInteractable)
        {
            _button.interactable = isInteractable;
        }

        public void SetShadowColor(Color color)
        {
            foreach (var shadow in _shadows)
            {
                shadow.color = color;
            }
        }

        public void SetDefaultShadowColor()
        {
            SetShadowColor(_uiSceneController.DefaultStateButtonColor);
        }

        public void SetDisabledShadowColor()
        {
            SetShadowColor(_uiSceneController.DisabledStateButtonColor);
        }
    }
}
