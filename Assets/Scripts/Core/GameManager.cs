using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Movement;
using EchoOfTheTimes.ScriptableObjects;
using EchoOfTheTimes.UI;
using EchoOfTheTimes.Units;
using EchoOfTheTimes.Utils;
using UnityEngine;

namespace EchoOfTheTimes.Core
{
    public class GameManager : MonoBehaviour
    {
        [field: Header("Parameters")]
        [field: SerializeField]
        public float TimeToChangeState_sec { get; private set; }

        [Header("Systems")]
        public LevelStateMachine StateMachine;
        public GraphVisibility Graph;
        public CheckpointManager CheckpointManager;

        [Header("Player")]
        public Player Player;
        public VertexFollower VertexFollower;
        public UserInput UserInput;
        public UserInputHandler UserInputHandler;

        [Header("Scriptable Objects")]
        public ColorStateSettingsScriptableObject ColorStateSettings;

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
             
            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            StateMachine.OnTransitionStart += Graph.ResetVertices;
            StateMachine.OnTransitionStart += StateMachine.StartTransition;

            StateMachine.OnTransitionComplete += Graph.Load;
            StateMachine.OnTransitionComplete += VertexFollower.Unlink;
            StateMachine.OnTransitionComplete += StateMachine.CompleteTransition;
        }

        private void UnsubscribeEvents()
        {
            StateMachine.OnTransitionStart -= Graph.ResetVertices;
            StateMachine.OnTransitionStart -= StateMachine.StartTransition;

            StateMachine.OnTransitionComplete -= Graph.Load;
            StateMachine.OnTransitionComplete -= VertexFollower.Unlink;
            StateMachine.OnTransitionComplete -= StateMachine.CompleteTransition;
        }
    }
}