using EchoOfTheTimes.Commands;
using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Movement;
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
        public CommandManager CommandManager;
        public UserInput UserInput;
        public CheckpointManager CheckpointManager;

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
            //StateMachine.OnTransitionStart += CommandManager.ForceStop;
            //StateMachine.OnTransitionStart += Player.MarkAsNeedStop;
            //StateMachine.OnTransitionStart += VertexFollower.LinkPlayer;
            StateMachine.OnTransitionStart += Graph.ResetVertices;
            StateMachine.OnTransitionStart += StateMachine.StartTransition;

            StateMachine.OnTransitionComplete += Graph.Load;
            StateMachine.OnTransitionComplete += VertexFollower.Unlink;
            StateMachine.OnTransitionComplete += StateMachine.CompleteTransition;
        }

        private void UnsubscribeEvents()
        {
            //StateMachine.OnTransitionStart -= CommandManager.ForceStop;
            //StateMachine.OnTransitionStart += Player.MarkAsNeedStop;
            //StateMachine.OnTransitionStart -= VertexFollower.LinkPlayer;
            StateMachine.OnTransitionStart -= Graph.ResetVertices;
            StateMachine.OnTransitionStart -= StateMachine.StartTransition;

            StateMachine.OnTransitionComplete -= Graph.Load;
            StateMachine.OnTransitionComplete -= VertexFollower.Unlink;
            StateMachine.OnTransitionComplete -= StateMachine.CompleteTransition;
        }
    }
}