using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Movement;
using EchoOfTheTimes.ScriptableObjects;
using EchoOfTheTimes.UI;
using EchoOfTheTimes.Units;
using EchoOfTheTimes.Utils;
using UnityEngine;

namespace EchoOfTheTimes.Core
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        [field: SerializeField]
        public float TimeToChangeState_sec { get; private set; }

        public LevelStateMachine StateMachine;
        public GraphVisibility Graph;
        public VertexFollower VertexFollower;
        public UserInputHandler UserInputHandler;
        public Player Player;
        public UserInput UserInput;
        public CheckpointManager CheckpointManager;
        public ColorStateSettingsScriptableObject ColorStateSettings;

        protected override void Awake()
        {
            base.Awake();

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