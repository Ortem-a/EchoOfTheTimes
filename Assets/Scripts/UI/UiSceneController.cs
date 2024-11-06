using DG.Tweening;
using EchoOfTheTimes.Core;
using EchoOfTheTimes.Effects;
using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Persistence;
using EchoOfTheTimes.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace EchoOfTheTimes.UI
{
    public class UiSceneController : MonoBehaviour
    {
        public bool flgIsStartAnimationEnded = false;

        [Header("Event система для отрубания")]
        [SerializeField] private EventSystem _eventSystem;

        [Header("Основной HUD")]
        [SerializeField] private Canvas HUDCanvas;
        private CanvasGroup hudCanvasGroup;

        [SerializeField] private Button ToMainMenuButton;
        [SerializeField] private Transform BottomPanel;
        [SerializeField] private Transform TopPanel;
        [SerializeField] private GameObject ButtonPrefab;

        [Header("Для цветов кнопок состояний")]
        public Color DefaultStateButtonColor;
        public Color DisabledStateButtonColor;
        [SerializeField] private Color _lineDopColor;
        [SerializeField] private Color _eyeColor;
        [SerializeField] private Color _backColor;
        [SerializeField] private Color _linesColor;
        [SerializeField] private Color _exitButton;

        [Header("Для аниматоров кнопок состояний")]
        [SerializeField] private RuntimeAnimatorController[] ButtonControllers;

        [Header("Обучающий UI")]
        [SerializeField] private RectTransform trainingPanel; // Обучающая панель, только на первом уровне первой главы

        [Header("Стартовый/финишный UI")]
        [SerializeField] private RectTransform rectFadeInOutPanel;
        [SerializeField] private float StartFadeInDuration_sec = 1f; // Длительность появления
        [SerializeField] private float StartDelay_sec = 0.5f; // Задержка перед началом
        [SerializeField] private float HUDStartBeforeEnd_sec = 1f; // Начало появления HUD за K секунд до конца

        [Header("Финишный UI")]
        [SerializeField] private float UselessFinishDuration_sec;
        [SerializeField] private float FinishFadeOutDuration_sec;

        private SceneLoader _loader;
        private LevelStateMachine _stateMachine;
        private PlayerProgressHudView _sceneView;
        private InputMediator _inputMediator;
        private LevelAudioManager _levelAudioManager;
        private HUDController _hudController;

        private UiStateButton[] _stateButtons;

        private void Start()
        {
            // Отключаем считывание нажатий на экран
            _eventSystem.enabled = false;

            // Прячем HUD
            InitializeHUD();

            // Показываем растемнение + включаем музон на фон
            ShowStartLevelCanvas();

            // Красим кнопку выхода из уровня
            ApplyExitButtonColor();
        }

        [Inject]
        private void Construct(LevelStateMachine stateMachine, PlayerProgressHudView uiSceneView, InputMediator inputMediator, LevelAudioManager levelAudioManager, HUDController hudController)
        {
            _stateMachine = stateMachine;
            _sceneView = uiSceneView;
            _inputMediator = inputMediator;
            _levelAudioManager = levelAudioManager;
            _hudController = hudController;

            CreateStateButtons();
            InitializeHUDCanvasGroup();

            _loader = FindObjectOfType<SceneLoader>();
        }

        private void InitializeHUD()
        {
            HUDCanvas.gameObject.SetActive(false);
            rectFadeInOutPanel.GetComponent<CanvasGroup>().alpha = 1f; // Начальная непрозрачность

            if (trainingPanel != null)
            {
                trainingPanel.GetComponent<CanvasGroup>().alpha = 0f;
            }
        }

        private void InitializeHUDCanvasGroup()
        {
            hudCanvasGroup = HUDCanvas.GetComponent<CanvasGroup>();
        }

        private void ApplyExitButtonColor()
        {
            var buttonImage = ToMainMenuButton.GetComponent<Image>();
            buttonImage.color = _exitButton;
        }

        private void CreateStateButtons()
        {
            _stateButtons = new UiStateButton[_stateMachine.States.Count];
            for (int i = 0; i < _stateMachine.States.Count; i++)
            {
                var stateButton = Instantiate(ButtonPrefab, BottomPanel).GetComponent<UiStateButton>();
                stateButton.Init(i, _inputMediator, this, _hudController, ButtonControllers[i],
                    _lineDopColor, _eyeColor, _backColor, _linesColor);
                _stateButtons[i] = stateButton;
            }
            _stateButtons[0].Select();
        }

        // Вызываем при нажатии кнопки выхода с уровня
        public void ExitToMainMenu()
        {
            PersistenceService.OnExitToMainMenu?.Invoke();

            CanvasGroup CanvasGroupFadeInOutPanel = rectFadeInOutPanel.GetComponent<CanvasGroup>();
            CanvasGroup canvasGroupBottomPanel = BottomPanel.GetComponent<CanvasGroup>();
            CanvasGroup canvasGroupTopPanel = TopPanel.GetComponent<CanvasGroup>();
            CanvasGroup CanvasGroupTrainingPanel = trainingPanel != null ? trainingPanel.GetComponent<CanvasGroup>() : null;

            _eventSystem.enabled = false;
            flgIsStartAnimationEnded = false;

            CanvasGroupFadeInOutPanel.gameObject.SetActive(true);
            canvasGroupBottomPanel.gameObject.SetActive(true);
            canvasGroupTopPanel.gameObject.SetActive(true);

            // Останавливаем эмбиент-звук с затуханием перед началом затемнения
            _levelAudioManager.StopAmbientSound();

            // Создаем последовательность для анимации панелей
            Sequence fadeOutSequence = DOTween.Sequence();

            // Анимация затемнения для FadeInOutPanel, BottomPanel, TopPanel и (опционально) TrainingPanel
            fadeOutSequence
                .Append(DOTween.To(() => CanvasGroupFadeInOutPanel.alpha, x => CanvasGroupFadeInOutPanel.alpha = x, 1f, FinishFadeOutDuration_sec)
                    .SetDelay(UselessFinishDuration_sec)
                    .SetEase(Ease.InOutQuad))
                .Join(DOTween.To(() => canvasGroupBottomPanel.alpha, x => canvasGroupBottomPanel.alpha = x, 0f, FinishFadeOutDuration_sec / 2)
                    .SetDelay(UselessFinishDuration_sec)
                    .SetEase(Ease.InOutQuad))
                .Join(DOTween.To(() => canvasGroupTopPanel.alpha, x => canvasGroupTopPanel.alpha = x, 0f, FinishFadeOutDuration_sec / 2)
                    .SetDelay(UselessFinishDuration_sec)
                    .SetEase(Ease.InOutQuad));

            // Проверка на наличие CanvasGroupTrainingPanel и добавление её анимации исчезновения
            if (CanvasGroupTrainingPanel != null)
            {
                fadeOutSequence.Join(DOTween.To(() => CanvasGroupTrainingPanel.alpha, x => CanvasGroupTrainingPanel.alpha = x, 0f, FinishFadeOutDuration_sec)
                    .SetDelay(UselessFinishDuration_sec)
                    .SetEase(Ease.InOutQuad));
            }

            fadeOutSequence.OnComplete(() => _loader.LoadMainMenuSceneAsync()); // Переход в меню по завершении анимации
        }


        // Запускаем при старте уровня
        public void ShowStartLevelCanvas()
        {
            // StartLevelCanvas.gameObject.SetActive(true);
            CanvasGroup CanvasGroupFadeInOutPanel = rectFadeInOutPanel.GetComponent<CanvasGroup>();
            CanvasGroup CanvasGroupTrainingPanel = trainingPanel != null ? trainingPanel.GetComponent<CanvasGroup>() : null;

            CanvasGroupFadeInOutPanel.alpha = 1f;
            if (CanvasGroupTrainingPanel != null)
            {
                CanvasGroupTrainingPanel.alpha = 0f;
            }

            // Перенёс сюда, чтобы не дропался ФПС на глазах у игрока при загрузке музыки
            _levelAudioManager.PlayAmbientSound();

            // Запуск анимации появления стартового экрана
            DOTween.To(() => CanvasGroupFadeInOutPanel.alpha, x => CanvasGroupFadeInOutPanel.alpha = x, 0f, StartFadeInDuration_sec)
                .SetDelay(StartDelay_sec)
                .OnStart(() => HUDCanvas.gameObject.SetActive(false))
                .OnUpdate(() =>
                {
                    // Синхронизация появления HUDCanvas и исчезновения CanvasGroupFadeInOutPanel
                    float elapsedFadeTime = StartFadeInDuration_sec * (1f - CanvasGroupFadeInOutPanel.alpha); // Прошедшее время анимации

                    if (elapsedFadeTime >= StartFadeInDuration_sec / 2)
                    {
                        HUDCanvas.gameObject.SetActive(true);
                        float hudAlphaProgress = (elapsedFadeTime - StartFadeInDuration_sec / 2) / (StartFadeInDuration_sec / 2);
                        hudCanvasGroup.alpha = Mathf.Lerp(0f, 1f, hudAlphaProgress);
                    }

                    // Синхронное появление CanvasGroupTrainingPanel и исчезновение CanvasGroupFadeInOutPanel (если TrainingPanel существует)
                    if (CanvasGroupTrainingPanel != null)
                    {
                        CanvasGroupTrainingPanel.alpha = 1f - CanvasGroupFadeInOutPanel.alpha;
                    }
                })
                .OnComplete(() =>
                {
                    CanvasGroupFadeInOutPanel.gameObject.SetActive(false);
                    HUDCanvas.gameObject.SetActive(true);
                    flgIsStartAnimationEnded = true;
                    //_eventSystem.enabled = true;

                    DOTween.Sequence()
                           .AppendInterval(0.53f)
                           .AppendCallback(() => _eventSystem.enabled = true);
                });
        }

        // Запускаем при достижении финишного вертекса
        public void EnableFinishCanvas()
        {
            CanvasGroup CanvasGroupFadeInOutPanel = rectFadeInOutPanel.GetComponent<CanvasGroup>();
            CanvasGroup canvasGroupBottomPanel = BottomPanel.GetComponent<CanvasGroup>();
            CanvasGroup canvasGroupTopPanel = TopPanel.GetComponent<CanvasGroup>();
            CanvasGroup CanvasGroupTrainingPanel = trainingPanel != null ? trainingPanel.GetComponent<CanvasGroup>() : null;

            _eventSystem.enabled = false;
            flgIsStartAnimationEnded = false;

            CanvasGroupFadeInOutPanel.gameObject.SetActive(true);
            canvasGroupBottomPanel.gameObject.SetActive(true);
            canvasGroupTopPanel.gameObject.SetActive(true);

            // Останавливаем эмбиент-звук с затуханием перед началом затемнения
            _levelAudioManager.StopAmbientSound();

            // Создаем последовательность для анимации панелей
            Sequence fadeOutSequence = DOTween.Sequence();

            // Анимация затемнения для FadeInOutPanel, BottomPanel, TopPanel и (опционально) TrainingPanel
            fadeOutSequence
                .Append(DOTween.To(() => CanvasGroupFadeInOutPanel.alpha, x => CanvasGroupFadeInOutPanel.alpha = x, 1f, FinishFadeOutDuration_sec)
                    .SetDelay(UselessFinishDuration_sec)
                    .SetEase(Ease.InOutQuad))
                .Join(DOTween.To(() => canvasGroupBottomPanel.alpha, x => canvasGroupBottomPanel.alpha = x, 0f, FinishFadeOutDuration_sec / 2)
                    .SetDelay(UselessFinishDuration_sec)
                    .SetEase(Ease.InOutQuad))
                .Join(DOTween.To(() => canvasGroupTopPanel.alpha, x => canvasGroupTopPanel.alpha = x, 0f, FinishFadeOutDuration_sec / 2)
                    .SetDelay(UselessFinishDuration_sec)
                    .SetEase(Ease.InOutQuad));

            // Проверка на наличие CanvasGroupTrainingPanel и добавление её анимации исчезновения
            if (CanvasGroupTrainingPanel != null)
            {
                fadeOutSequence.Join(DOTween.To(() => CanvasGroupTrainingPanel.alpha, x => CanvasGroupTrainingPanel.alpha = x, 0f, FinishFadeOutDuration_sec)
                    .SetDelay(UselessFinishDuration_sec)
                    .SetEase(Ease.InOutQuad));
            }

            fadeOutSequence.OnComplete(() =>
            {
                if (_loader.HasNextLevel)
                {
                    Debug.Log("ПЕРЕД ВЫЗОВОМ НЕКСТ СЦЕНЫ");
                    _loader.LoadNextSceneGroupAsync();
                }
                else
                {
                    ToMainMenuButton.onClick?.Invoke();
                }
            });
        }

        public void DeselectAllButtons(int exceptIndex)
        {
            for (int i = 0; i < _stateButtons.Length; i++)
            {
                if (i == exceptIndex) continue;
                _stateButtons[i].Deselect();
            }
        }
    }
}
