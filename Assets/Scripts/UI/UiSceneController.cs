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
        public Transform BottomPanel;
        public GameObject ButtonPrefab;

        [Header("Finish UI")]
        public Canvas FinishCanvas;
        public Transform FinishPanel;
        public Button FinishButton;

        [Header("Start Level UI")]
        public Canvas StartLevelCanvas;

        private SceneLoader _loader;
        private LevelStateMachine _stateMachine;

        private UiSceneView _sceneView;

        [Inject]
        private void Construct(LevelStateMachine stateMachine, UiSceneView uiSceneView, InputMediator inputHandler)
        {
            _stateMachine = stateMachine;
            _sceneView = uiSceneView;

            for (int i = 0; i < _stateMachine.States.Count; i++)
            {
                var obj = Instantiate(ButtonPrefab, BottomPanel);
                obj.GetComponent<UiStateButton>().Init(i, inputHandler);
            }

            FinishPanel.localScale = Vector3.zero;

            _loader = FindObjectOfType<SceneLoader>();

            ToMainMenuButton.onClick.AddListener(ExitToMainMenu);
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

        public void UpdateLabel()
        {
            int stateId = _stateMachine.GetCurrentStateId();
            _sceneView.UpdateLabel(stateId);
        }

        public void EnableFinishCanvas()
        {
            SetActiveBottomPanel(false);

            FinishCanvas.gameObject.SetActive(true);
            FinishPanel.DOScale(1f, 0.5f);
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

        private void Start()
        {
            HUDCanvas.gameObject.SetActive(false);
            ShowStartLevelCanvas();
        }
    }
}
