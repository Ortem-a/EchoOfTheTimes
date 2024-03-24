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
            FinishPanel.DOScale(0f, 0f);

            _stateMachine = GameManager.Instance.StateMachine;
            _sceneView = UiManager.Instance.UiSceneView;

            for (int i = 0; i < _stateMachine.States.Count; i++)
            {
                var obj = Instantiate(ButtonPrefab, BottomPanel);
                obj.GetComponent<UiButtonController>().Initialize(i);
            }
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
            BottomPanel.gameObject.SetActive(false);
            FinishCanvas.gameObject.SetActive(true);

            FinishPanel.DOScale(1f, 0.5f);
        }
    }
}