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
        public Button ToMainMenuButton;
        public Button ToCheckpointButton;
        public Transform BottomPanel;
        public GameObject ButtonPrefab;

        [Header("Finish UI")]
        public Canvas FinishCanvas;
        public Transform FinishPanel;
        public Button FinishButton;

        private SceneLoader _loader;
        private LevelStateMachine _stateMachine;

        private UiSceneView _sceneView;

        private void Awake()
        {
            _loader = FindObjectOfType<SceneLoader>();

            FinishCanvas.gameObject.SetActive(false);

            ToMainMenuButton.onClick.AddListener(ExitToMainMenu);
            FinishButton.onClick.AddListener(ExitToMainMenu);
            ToCheckpointButton.onClick.AddListener(GoToCheckpoint);
        }

        [Inject]
        private void Initialize(LevelStateMachine stateMachine, UiSceneView uiSceneView, UserInputHandler inputHandler)
        {
            _stateMachine = stateMachine;
            _sceneView = uiSceneView;

            for (int i = 0; i < _stateMachine.States.Count; i++)
            {
                var obj = Instantiate(ButtonPrefab, BottomPanel);
                obj.GetComponent<UiButtonController>().Initialize(i, inputHandler);
            }

            FinishPanel.DOScale(0f, 0f);
        }

        public void Initialize()
        {
            _stateMachine = GameManager.Instance.StateMachine;
            _sceneView = UiManager.Instance.UiSceneView;

            for (int i = 0; i < _stateMachine.States.Count; i++)
            {
                var obj = Instantiate(ButtonPrefab, BottomPanel);
                obj.GetComponent<UiButtonController>().Initialize(i);
            }

            FinishPanel.DOScale(0f, 0f);
        }

        private void ExitToMainMenu()
        {
            _loader.LoadSceneGroupAsync(0);
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

        public void GoToCheckpoint()
        {
            GameManager.Instance.UserInputHandler.GoToCheckpoint();
        }

        public void EnableCheckpointButton()
        {
            ToCheckpointButton.transform.DOScale(0f, 0f);

            ToCheckpointButton.gameObject.SetActive(true);

            ToCheckpointButton.transform.DOScale(1f, 0.2f);
        }
    }
}