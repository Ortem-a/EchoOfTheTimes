using EchoOfTheTimes.Commands;
using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Movement;
using EchoOfTheTimes.Units;
using UnityEngine;

namespace EchoOfTheTimes.Core
{
    public class GameManager : MonoBehaviour
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

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
    }
}