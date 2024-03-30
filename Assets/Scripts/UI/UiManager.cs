using EchoOfTheTimes.Core;
using UnityEngine;

namespace EchoOfTheTimes.UI
{
    public class UiManager : MonoBehaviour
    {
        [Header("Controllers")]
        public UiSceneController UiSceneController;

        [Header("Views")]
        public UiSceneView UiSceneView;

        public static UiManager Instance { get; private set; }

        public void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            SubscribeEvents();

            UiSceneController.Initialize();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            GameManager.Instance.StateMachine.OnTransitionComplete += UiSceneController.UpdateLabel;
        }

        private void UnsubscribeEvents()
        {
            GameManager.Instance.StateMachine.OnTransitionComplete -= UiSceneController.UpdateLabel;
        }
    }
}