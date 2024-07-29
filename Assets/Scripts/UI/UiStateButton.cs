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
            HUDController hudController, RuntimeAnimatorController animatorController,
            Color lineDopColor, Color eyeColor, Color backColor, Color linesColor)
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

            SetFuckingColorToFuckingButton(lineDopColor, eyeColor, backColor, linesColor);
        }

        private void ChangeState(int stateId)
        {
            // Проверяем, прошло ли 1 секунда после спавна
            // ЭТО КОСТЫЛЬ ЧТОБЫ БЛЯТЬ СУКА ГЛАЗА ОТКРЫЛИСЬ ЕБУЧИЕ УУУУУУУУУУУУУ
            if (Time.time - _spawnTime < 2f) return;

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

        private void SetShadowColor(Color color)
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

        private void SetFuckingColorToFuckingButton(Color lineDopColor, Color eyeColor, Color backColor, Color linesColor)
        {
            transform.GetChild(4).GetComponent<Image>().color = lineDopColor;
            transform.GetChild(5).GetComponent<Image>().color = backColor;
            transform.GetChild(6).GetComponent<Image>().color = eyeColor;
            transform.GetChild(7).GetComponent<Image>().color = linesColor;
        }
    }
}
