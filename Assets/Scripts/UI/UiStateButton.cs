using EchoOfTheTimes.Core;
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

        private Color _defaultColor;
        private Color _disabledColor;
        private Image[] _shadows;

        public void Init(int stateId, InputMediator inputHandler, UiSceneController uiSceneController,
            AnimatorController animatorController, Color defaultColor, Color disabledColor)
        {
            _inputMediator = inputHandler;
            _uiSceneController = uiSceneController;

            _button = GetComponent<Button>();
            //_button.transition = Selectable.Transition.Animation;
            _button.transition = Selectable.Transition.None;
            _button.onClick.AddListener(() => ChangeState(stateId));

            _animator = GetComponent<Animator>();
            _animator.runtimeAnimatorController = animatorController;

            _defaultColor = defaultColor;
            _disabledColor = disabledColor;

            _shadows = new Image[4];
            _shadows[0] = transform.GetChild(0).GetComponent<Image>();
            _shadows[1] = transform.GetChild(1).GetComponent<Image>();
            _shadows[2] = transform.GetChild(2).GetComponent<Image>();
            _shadows[3] = transform.GetChild(3).GetComponent<Image>();

            SetInteractable(true);
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

            if (isInteractable)
            {
                for (int i = 0; i < _shadows.Length; i++)
                    _shadows[i].color = _defaultColor;
            }
            else
            {
                for (int i = 0; i < _shadows.Length; i++)
                    _shadows[i].color = _disabledColor;
            }
        }
    }
}