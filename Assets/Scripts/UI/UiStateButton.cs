using EchoOfTheTimes.Core;
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

            _hudController.RegisterButton(this);

            _spawnTime = Time.time;  // Записываем время спавна кнопки
        }

        private void ChangeState(int stateId)
        {
            if (Time.time - _spawnTime < 3f) return; // Проверяем, прошло ли 1 секунда после спавна

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
    }
}
