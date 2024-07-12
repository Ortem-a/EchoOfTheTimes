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
        }

        private void ChangeState(int stateId)
        {
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
