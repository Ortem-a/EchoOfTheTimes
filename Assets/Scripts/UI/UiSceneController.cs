using DG.Tweening;
using EchoOfTheTimes.Core;
using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.SceneManagement;
using UnityEngine;
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

        public Color DefaultStateButtonColor;
        public Color DisabledStateButtonColor;

        [Header("Buttons Animator Controllers")]
        public RuntimeAnimatorController[] ButtonControllers;

        [Header("Finish UI")]
        public Canvas FinishCanvas;
        public Transform FinishPanel;
        public Button FinishButton;
        public CanvasGroup FinishFadeOutPanel;
        public float UselessFinishDuration_sec;
        public float FinishFadeOutDuration_sec;

        [Header("Start Level UI")]
        public Canvas StartLevelCanvas;
        public CanvasGroup StartFadeInPanel; // CanvasGroup для плавного появления
        public float StartFadeInDuration_sec = 2.0f; // Длительность появления
        public float StartDelay_sec = 1.0f; // Задержка перед началом
        public float HUDStartBeforeEnd_sec = 0.5f; // Начало появления HUD за K секунд до конца

        private SceneLoader _loader;
        private LevelStateMachine _stateMachine;
        private UiSceneView _sceneView;
        private InputMediator _inputMediator;

        private UiStateButton[] _stateButtons;

        private void Start()
        {
            InitializeHUD();
            ShowStartLevelCanvas();
        }

        [Inject]
        private void Construct(LevelStateMachine stateMachine, UiSceneView uiSceneView, InputMediator inputMediator)
        {
            _stateMachine = stateMachine;
            _sceneView = uiSceneView;
            _inputMediator = inputMediator;

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

        private void CreateStateButtons()
        {
            _stateButtons = new UiStateButton[_stateMachine.States.Count];
            for (int i = 0; i < _stateMachine.States.Count; i++)
            {
                var stateButton = Instantiate(ButtonPrefab, BottomPanel).GetComponent<UiStateButton>();
                stateButton.Init(i, _inputMediator, this, FindObjectOfType<HUDController>(), ButtonControllers[i]);
                _stateButtons[i] = stateButton;
            }
            _stateButtons[0].Select();
        }


        private async void ExitToMainMenu()
        {
            await _loader.LoadSceneGroupAsync(0);
        }

        private async void GoToNextLevel()
        {
            await _loader.LoadNextSceneGroupAsync();
        }

        public void UpdateLabel()
        {
            int stateId = _stateMachine.GetCurrentStateId();
            _sceneView.UpdateLabel(stateId);
        }

        public void EnableFinishCanvas()
        {
            SetActiveHudImmediate(false);
            _inputMediator.gameObject.SetActive(false);

            FinishFadeOutPanel.gameObject.SetActive(true);

            DOTween.To(() => FinishFadeOutPanel.alpha, x => FinishFadeOutPanel.alpha = x, 1f, FinishFadeOutDuration_sec)
                .SetDelay(UselessFinishDuration_sec)
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

        public void SetActiveBottomPanel(bool isActive, float duration = 0.2f)
        {
            for (int i = 0; i < _stateButtons.Length; i++)
            {
                _stateButtons[i].SetInteractable(isActive);
            }

            //if (isActive)
            //{
            //    BottomPanel.DOScale(1f, duration)
            //        .OnStart(() => BottomPanel.gameObject.SetActive(isActive));
            //}
            //else
            //{
            //    BottomPanel.DOScale(0f, duration)
            //        .OnComplete(() => BottomPanel.gameObject.SetActive(isActive));
            //}
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

            // Запуск анимации появления стартового экрана
            DOTween.To(() => StartFadeInPanel.alpha, x => StartFadeInPanel.alpha = x, 0f, StartFadeInDuration_sec)
                .SetDelay(StartDelay_sec)
                .OnStart(() =>
                {
                    HUDCanvas.gameObject.SetActive(false);
                })
                .OnUpdate(() =>
                {
                    // Начало появления HUD за K секунд до окончания анимации стартового экрана
                    if (StartFadeInPanel.alpha <= HUDStartBeforeEnd_sec / StartFadeInDuration_sec)
                    {
                        HUDCanvas.gameObject.SetActive(true);
                        hudCanvasGroup.alpha = Mathf.Lerp(0f, 1f, (HUDStartBeforeEnd_sec - StartFadeInPanel.alpha * StartFadeInDuration_sec) / HUDStartBeforeEnd_sec);
                    }
                })
                .OnComplete(() =>
                {
                    StartLevelCanvas.gameObject.SetActive(false);
                    HUDCanvas.gameObject.SetActive(true);
                    flgIsStartAnimationEnded = true;
                    hudCanvasGroup.alpha = 1f; // Убедитесь, что HUD полностью виден
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
