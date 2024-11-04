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
        private float _spawnTime;

        private Image _shadow;

        private Color _goodShadowColor;
        private Color _badShadowColor;

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

            // ���������� ���� ������ ����
            _shadow = GetComponentsInChildren<Image>().FirstOrDefault(image => image.gameObject.name.Contains("Shadow"));

            // ������ ����� ��� ������� � ������ ����, ������� �����-��������
            _goodShadowColor = _uiSceneController.DefaultStateButtonColor;
            _badShadowColor = _uiSceneController.DisabledStateButtonColor;

            // ������������� ��������� ���� ����
            SetShadowColor(_badShadowColor);

            _hudController.RegisterButton(this);

            _spawnTime = Time.time;

            SetButtonColors(lineDopColor, eyeColor, backColor, linesColor);
        }

        private void ChangeState(int stateId)
        {
            if (Time.time - _spawnTime < 1.7f) return;

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

        private void SetShadowColor(Color color)
        {
            if (_shadow != null)
            {
                _shadow.color = color; // ������������� ���� � �����-�������� ����
            }
        }

        // ����� ��� ��������� ���� �� "�������"
        public void ChangeShadowToGood()
        {
            StartCoroutine(InterpolateShadowColor(_shadow.color, _goodShadowColor, 0.25f));
        }

        // ����� ��� ��������� ���� �� "������"
        public void ChangeShadowToBad()
        {
            StartCoroutine(InterpolateShadowColor(_shadow.color, _badShadowColor, 0.25f));
        }

        // �������� ��� �������� �������� ����� � �����-������ ����
        private IEnumerator InterpolateShadowColor(Color fromColor, Color toColor, float duration)
        {
            if (_shadow == null) yield break;

            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                Color newColor = Color.Lerp(fromColor, toColor, elapsed / duration); // �������� ������������ ���� ���������
                SetShadowColor(newColor); // ������������� ����� ���� ������ � �����
                yield return null;
            }

            SetShadowColor(toColor); // ������������� �������� ���� � �������� �����
        }

        private void SetButtonColors(Color lineDopColor, Color eyeColor, Color backColor, Color linesColor)
        {
            transform.GetChild(1).GetComponent<Image>().color = lineDopColor;
            transform.GetChild(2).GetComponent<Image>().color = backColor;
            transform.GetChild(3).GetComponent<Image>().color = eyeColor;
            transform.GetChild(4).GetComponent<Image>().color = linesColor;
        }
    }
}
