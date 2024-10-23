using DG.Tweening;
using EchoOfTheTimes.Core;
using EchoOfTheTimes.Effects;
using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Persistence;
using EchoOfTheTimes.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace EchoOfTheTimes.UI
{
    public class UiSceneController : MonoBehaviour
    {
        public bool flgIsStartAnimationEnded = false;

        [Header("HUD")]
        public Canvas HUDCanvas;
        private CanvasGroup hudCanvasGroup;

        public Button ToMainMenuButton;
        public Button ToNextLevelButton;
        public Transform BottomPanel;
        public Transform TopPanel;
        public GameObject ButtonPrefab;

        [Header("Button color settings")]
        public Color DefaultStateButtonColor;
        public Color DisabledStateButtonColor;
        [SerializeField]
        private Color _lineDopColor;
        [SerializeField]
        private Color _eyeColor;
        [SerializeField]
        private Color _backColor;
        [SerializeField]
        private Color _linesColor;
        [SerializeField]
        private Color _exitButton;

        [Header("Buttons Animator Controllers")]
        public RuntimeAnimatorController[] ButtonControllers;

        [Header("Start Level UI")]
        public Canvas StartLevelCanvas;
        public CanvasGroup StartFadeInPanel; // CanvasGroup для плавного появления
        private float StartFadeInDuration_sec = 2f; // Длительность появления
        private float StartDelay_sec = 0.5f; // Задержка перед началом
        private float HUDStartBeforeEnd_sec = 1f; // Начало появления HUD за K секунд до конца

        [Header("Finish UI")]
        public Canvas FinishCanvas;
        public Transform FinishPanel;
        public Button FinishButton;
        public CanvasGroup FinishFadeOutPanel;
        public float UselessFinishDuration_sec;
        public float FinishFadeOutDuration_sec;

        private SceneLoader _loader;
        private LevelStateMachine _stateMachine;
        private PlayerProgressHudView _sceneView;
        private InputMediator _inputMediator;
        private LevelAudioManager _levelAudioManager;
        private HUDController _hudController;

        private UiStateButton[] _stateButtons;

        private void Start()
        {
            InitializeHUD();
            ShowStartLevelCanvas();

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
            InitializeFinishCanvas();
            InitializeHUDCanvasGroup();

            _loader = FindObjectOfType<SceneLoader>();

            ToMainMenuButton.onClick.AddListener(ExitToMainMenu);
            ToNextLevelButton.onClick.AddListener(GoToNextLevel);
            FinishButton.onClick.AddListener(ExitToMainMenu);
        }

        private void InitializeHUD()
        {
            HUDCanvas.gameObject.SetActive(false);
            StartFadeInPanel.alpha = 1f; // Начальная непрозрачность
        }

        private void InitializeHUDCanvasGroup()
        {
            hudCanvasGroup = HUDCanvas.GetComponent<CanvasGroup>();
            if (hudCanvasGroup == null)
            {
                hudCanvasGroup = HUDCanvas.gameObject.AddComponent<CanvasGroup>();
            }
        }

        private void InitializeFinishCanvas()
        {
            FinishPanel.localScale = Vector3.zero;
            FinishCanvas.gameObject.SetActive(false);
            FinishFadeOutPanel.alpha = 0f;
            FinishFadeOutPanel.gameObject.SetActive(false);
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

        private async void ExitToMainMenu()
        {
            PersistenceService.OnExitToMainMenu?.Invoke();

            await _loader.LoadMainMenuSceneAsync();

            //await _loader.LoadSceneGroupAsync(0);
        }

        private async void GoToNextLevel()
        {
            await _loader.LoadNextSceneGroupAsync();
        }

        //public void UpdateLabel()
        //{
        //    int stateId = _stateMachine.GetCurrentStateId();
        //    _sceneView.UpdateProgress(stateId);
        //}

        public void EnableFinishCanvas()
        {
            SetActiveHudImmediate(false);
            _inputMediator.gameObject.SetActive(false);

            FinishFadeOutPanel.gameObject.SetActive(true);

            // Останавливаем эмбиент-звук с затуханием перед началом затемнения
            if (_levelAudioManager != null)
            {
                _levelAudioManager.StopAmbientSound();
            }

            DOTween.To(() => FinishFadeOutPanel.alpha, x => FinishFadeOutPanel.alpha = x, 1f, FinishFadeOutDuration_sec)
                .SetDelay(UselessFinishDuration_sec)
                .OnUpdate(() => {
                    Debug.Log("Альфа во время обновления ФИНИША: " + FinishFadeOutPanel.alpha);
                })
                .OnComplete(() =>
                {
                    if (_loader.HasNextLevel)
                    {
                        ToNextLevelButton.onClick?.Invoke();
                    }
                    else
                    {
                        ToMainMenuButton.onClick?.Invoke();
                    }
                });
        }

        public void SetActiveBottomPanel(bool isActive)
        {
            for (int i = 0; i < _stateButtons.Length; i++)
            {
                _stateButtons[i].ChangeInteractable(isActive);
            }
        }

        public void SetActiveBottomPanelImmediate(bool isActive)
        {
            BottomPanel.gameObject.SetActive(isActive);
        }

        public void SetActiveTopPanelImmediate(bool isActive)
        {
            TopPanel.gameObject.SetActive(isActive);
        }

        public void ShowStartLevelCanvas()
        {
            StartLevelCanvas.gameObject.SetActive(true);
            StartFadeInPanel.alpha = 1f;

            // Перенёс сюды чтобы не дропался ФПС на глазах у игрока при загрузке музыки
            _levelAudioManager.PlayAmbientSound();

            // Активируем BottomPanel сразу с началом анимации спадения темноты
            SetActiveBottomPanelImmediate(true);

            // Запуск анимации появления стартового экрана
            DOTween.To(() => StartFadeInPanel.alpha, x => StartFadeInPanel.alpha = x, 0f, StartFadeInDuration_sec)
                .SetDelay(StartDelay_sec)
                .OnStart(() =>
                {
                    HUDCanvas.gameObject.SetActive(false);

                    //_levelAudioManager.PlayAmbientSound(SceneManager.GetActiveScene().name);
                    //_levelAudioManager.PlayAmbientSound();
                })
                .OnUpdate(() =>
                {
                    // Плавное появление HUD
                    if (StartFadeInPanel.alpha <= 1)
                    {
                        HUDCanvas.gameObject.SetActive(true);
                        hudCanvasGroup.alpha = Mathf.Lerp(0f, 1f, (HUDStartBeforeEnd_sec - StartFadeInPanel.alpha * StartFadeInDuration_sec) / HUDStartBeforeEnd_sec * 2);
                    }

                    Debug.Log("Альфа во время обновления: " + StartFadeInPanel.alpha);
                })
                .OnComplete(() =>
                {
                    StartLevelCanvas.gameObject.SetActive(false);
                    HUDCanvas.gameObject.SetActive(true);
                    flgIsStartAnimationEnded = true;
                    hudCanvasGroup.alpha = 1f;
                });
        }

        public void SetActiveHudImmediate(bool isActive)
        {
            SetActiveBottomPanelImmediate(isActive);
            SetActiveTopPanelImmediate(isActive);
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
