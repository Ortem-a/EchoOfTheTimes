using DG.Tweening;
using EchoOfTheTimes.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EchoOfTheTimes.UI.MainMenu
{
    [RequireComponent(typeof(Button))]
    public class LevelButtonHandler : MonoBehaviour
    {
        private Button _button;
        private SceneLoader _sceneLoader;
        private GameLevel _levelData;

        [SerializeField] private EventSystem _eventSystem;

        [SerializeField] private float scaleFactor = 1.1f; // Величина увеличения
        [SerializeField] private float animationDuration = 0.3f; // Общее время анимации для кнопки
        [SerializeField] private RectTransform FadeInOutPanel; // Панель для затемнения
        [SerializeField] private float fadeDuration = 0.6f; // Длительность затемнения

        [Inject]
        private void Construct(UiMainMenuService mainMenuService)
        {
            _sceneLoader = mainMenuService.SceneLoader;
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(HandleButtonClicked);
        }

        public void SetData(GameLevel levelData) => _levelData = levelData;

        private void HandleButtonClicked()
        {
            if (_levelData.LevelStatus == StatusType.Locked) return;

            Transform buttonTransform = transform;
            float halfDuration = animationDuration / 2;

            _eventSystem.enabled = false;

            // punk
            Sequence buttonPunkSequence = DOTween.Sequence();
            buttonPunkSequence
                .Append(buttonTransform.DOScale(scaleFactor, halfDuration).SetEase(Ease.OutQuad))
                .Append(buttonTransform.DOScale(1f, halfDuration).SetEase(Ease.InQuad));

            // Затемнение экрана
            CanvasGroup fadeCanvasGroup = FadeInOutPanel.GetComponent<CanvasGroup>();
            fadeCanvasGroup.alpha = 0f; 
            fadeCanvasGroup.gameObject.SetActive(true);

            DOTween.To(() => fadeCanvasGroup.alpha, x => fadeCanvasGroup.alpha = x, 1f, fadeDuration)
                .OnComplete(() =>
                {
                    _sceneLoader.LoadSceneGroupAsync(_levelData);
                });
        }
    }
}