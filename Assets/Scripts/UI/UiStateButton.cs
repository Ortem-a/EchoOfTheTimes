using EchoOfTheTimes.Core;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace EchoOfTheTimes.UI
{
    [RequireComponent(typeof(Button), typeof(Animator))]
    public class UiStateButton : MonoBehaviour
    {
        private InputMediator _inputMediator;
        private UiSceneController _uiSceneController;
        private Button _button;
        private Animator _animator;

        public void Init(int stateId, InputMediator inputHandler, UiSceneController uiSceneController,
            AnimatorController animatorController)
        {
            _inputMediator = inputHandler;
            _uiSceneController = uiSceneController;

            _button = GetComponent<Button>();
            _button.transition = Selectable.Transition.Animation;
            _button.onClick.AddListener(() => ChangeState(stateId));

            _animator = GetComponent<Animator>();
            _animator.runtimeAnimatorController = animatorController;
        }

        private void ChangeState(int stateId)
        {
            Select();
            _uiSceneController.DeselectAllButtons(stateId);

            _inputMediator.ChangeLevelState(stateId);
        }

        public void Deselect() => _animator.Play("Eye_ToClose");

        public void Select() => _animator.Play("Eye_ToOpen");
    }
}