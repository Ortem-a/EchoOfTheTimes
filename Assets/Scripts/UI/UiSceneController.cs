using DG.Tweening;
using EchoOfTheTimes.Core;
using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace EchoOfTheTimes.UI
{
    public class UiSceneController : MonoBehaviour
    {
        public Button ToMainMenuButton;
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

        public void SetActiveBottomPanel(bool isActive)
        {
            if (isActive)
            {
                BottomPanel.DOScale(1f, 0.2f)
                    .OnStart(() => BottomPanel.gameObject.SetActive(isActive));
            }
            else
            {
                BottomPanel.DOScale(0f, 0.2f)
                    .OnComplete(() => BottomPanel.gameObject.SetActive(isActive));
            }
        }
    }
}