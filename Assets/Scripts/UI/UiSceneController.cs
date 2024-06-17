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

        [Header("States Buttons")]
        [SerializeField]
        private Color _deselectedColor;
        [SerializeField]
        private Color _selectedColor;

        [Header("HUD")]
        public Canvas HUDCanvas;
        private CanvasGroup hudCanvasGroup;

        public Button ToMainMenuButton;
        public Button ToNextLevelButton;
        public Transform BottomPanel;
        public Transform TopPanel;
        public GameObject ButtonPrefab;

        [Header("Finish UI")]
        public Canvas FinishCanvas;
        public Transform FinishPanel;
        public Button FinishButton;
        public CanvasGroup FinishFadeOutPanel;
        public float FinishFadeOutDuration_sec;

        [Header("Start Level UI")]
        public Canvas StartLevelCanvas;

        private SceneLoader _loader;
        private LevelStateMachine _stateMachine;
        private UiSceneView _sceneView;
        private InputMediator _inputMediator;

        private UiStateButton[] _stateButtons;

        private void Start()
        {
            HUDCanvas.gameObject.SetActive(false);
            ShowStartLevelCanvas();
        }

        [Inject]
        private void Construct(LevelStateMachine stateMachine, UiSceneView uiSceneView, InputMediator inputMediator)
        {
            _stateMachine = stateMachine;
            _sceneView = uiSceneView;

            _inputMediator = inputMediator;

            _stateButtons = new UiStateButton[_stateMachine.States.Count];
            for (int i = 0; i < _stateMachine.States.Count; i++)
            {
                var stateButton = Instantiate(ButtonPrefab, BottomPanel).GetComponent<UiStateButton>();
                stateButton.Init(i, inputMediator, this, _deselectedColor, _selectedColor);
                _stateButtons[i] = stateButton;
            }

            FinishPanel.localScale = Vector3.zero;

            _loader = FindObjectOfType<SceneLoader>();

            ToMainMenuButton.onClick.AddListener(ExitToMainMenu);
            ToNextLevelButton.onClick.AddListener(GoToNextLevel);
            FinishButton.onClick.AddListener(ExitToMainMenu);

            FinishCanvas.gameObject.SetActive(false);
            StartLevelCanvas.gameObject.SetActive(false);

            hudCanvasGroup = HUDCanvas.GetComponent<CanvasGroup>();
            if (hudCanvasGroup == null)
            {
                hudCanvasGroup = HUDCanvas.gameObject.AddComponent<CanvasGroup>();
            }
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
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            SetActiveHudImmediate(false);
            _inputMediator.gameObject.SetActive(false);

            FinishFadeOutPanel.gameObject.SetActive(true);
            FinishFadeOutPanel.alpha = 0f;
            DOTween.To(() => FinishFadeOutPanel.alpha, x => FinishFadeOutPanel.alpha = x, 1f, FinishFadeOutDuration_sec)
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
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            // ====================================================================================
            // ЗДЕСЬ СПРЯТАЛ ВОЗМОЖНОСТЬ:
            // - ВЫХОДА В ГЛАВНОЕ МЕНЮ ПО ЗАВЕРШЕНИЮ УРОВНЯ
            // - ПЕРЕХОДА НА СЛЕДУЮЩИЙ УРОВЕНЬ ПО КНОПКЕ
            // ====================================================================================

            //SetActiveBottomPanel(false);

            //if (_loader.HasNextLevel)
            //{
            //    ToNextLevelButton.gameObject.SetActive(true);
            //}

            //FinishCanvas.gameObject.SetActive(true);
            //FinishPanel.DOScale(1f, 0.5f);
        }

        public void SetActiveBottomPanel(bool isActive, float duration = 0.2f)
        {
            if (isActive)
            {
                BottomPanel.DOScale(1f, duration)
                    .OnStart(() => BottomPanel.gameObject.SetActive(isActive));
            }
            else
            {
                BottomPanel.DOScale(0f, duration)
                    .OnComplete(() => BottomPanel.gameObject.SetActive(isActive));
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

        // Метод для показа StartLevelCanvas и запуска анимации
        public void ShowStartLevelCanvas()
        {
            StartLevelCanvas.gameObject.SetActive(true);
            Animator startLevelAnimator = StartLevelCanvas.GetComponent<Animator>();
            startLevelAnimator.Play("StartLevelAnimation"); // Предполагается, что "StartLevelAnimation" - это имя вашей анимации

            // Задержка для скрытия StartLevelCanvas и показа HUD после завершения анимации
            float animationDuration = startLevelAnimator.GetCurrentAnimatorStateInfo(0).length;
            DOVirtual.DelayedCall(animationDuration, () =>
            {
                StartLevelCanvas.gameObject.SetActive(false);
                HUDCanvas.gameObject.SetActive(true);
                flgIsStartAnimationEnded = true;
                FadeInHUD(); // Добавлено
            });
        }

        private void FadeInHUD()
        {
            hudCanvasGroup.alpha = 0f;
            DOTween.To(() => hudCanvasGroup.alpha, x => hudCanvasGroup.alpha = x, 1f, 1f); // Кастомная анимация
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
