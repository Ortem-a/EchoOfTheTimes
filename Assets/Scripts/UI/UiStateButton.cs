using EchoOfTheTimes.Core;
using System.Collections;
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

        private Image[] _shadowsGood;
        private Image[] _shadowsBad;

        private bool _isGoodShadowActive;

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

            _shadowsGood = GetComponentsInChildren<Image>().Where(image => image.gameObject.name.Contains("Shadow_Good")).ToArray();
            _shadowsBad = GetComponentsInChildren<Image>().Where(image => image.gameObject.name.Contains("Shadow_Bad")).ToArray();

            SetGoodShadowColor(_uiSceneController.DefaultStateButtonColor);
            SetBadShadowColor(_uiSceneController.DisabledStateButtonColor);

            // Устанавливаем начальную прозрачность теней
            SetAlpha(_shadowsGood, 1f);
            SetAlpha(_shadowsBad, 0f);

            _hudController.RegisterButton(this);

            _spawnTime = Time.time;  // Записываем время спавна кнопки

            SetFuckingColorToFuckingButton(lineDopColor, eyeColor, backColor, linesColor);

            _isGoodShadowActive = true;  // Изначально активны хорошие тени
        }

        private void ChangeState(int stateId)
        {
            // Проверяем, прошло ли 1 секунда после спавна
            if (Time.time - _spawnTime < 2f) return;

            Select();
            _uiSceneController.DeselectAllButtons(stateId);
            _inputMediator.ChangeLevelState(stateId);
        }

        public void Deselect() => _animator.SetBool("Tap", false);

        public void Select() => _animator.SetBool("Tap", true);

        public void ChangeInteractable(bool isInteractable)
        {
            _button.interactable = isInteractable;
        }

        private void SetGoodShadowColor(Color color)
        {
            foreach (var shadow in _shadowsGood)
            {
                shadow.color = color;
            }
        }

        private void SetBadShadowColor(Color color)
        {
            foreach (var shadow in _shadowsBad)
            {
                shadow.color = color;
            }
        }

        public void SetGoodShadowActive(bool isActive)
        {
            StartCoroutine(FadeShadow(_shadowsGood, isActive ? 1f : 0f, 0.25f));
        }

        public void SetBadShadowActive(bool isActive)
        {
            StartCoroutine(FadeShadow(_shadowsBad, isActive ? 1f : 0f, 0.25f));
        }

        private void SetAlpha(Image[] images, float alpha)
        {
            foreach (var image in images)
            {
                var color = image.color;
                color.a = alpha;
                image.color = color;
            }
        }

        private IEnumerator FadeShadow(Image[] images, float targetAlpha, float duration)
        {
            float startAlpha = images[0].color.a;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
                SetAlpha(images, newAlpha);
                yield return null;
            }

            SetAlpha(images, targetAlpha);
        }

        private void SetFuckingColorToFuckingButton(Color lineDopColor, Color eyeColor, Color backColor, Color linesColor)
        {
            transform.GetChild(8).GetComponent<Image>().color = lineDopColor;
            transform.GetChild(9).GetComponent<Image>().color = backColor;
            transform.GetChild(10).GetComponent<Image>().color = eyeColor;
            transform.GetChild(11).GetComponent<Image>().color = linesColor;
        }
    }
}
