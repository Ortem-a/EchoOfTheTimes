using EchoOfTheTimes.Commands;
using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Movement;
using EchoOfTheTimes.Units;
using UnityEngine;

namespace EchoOfTheTimes.Core
{
    public class LinksContainer : MonoBehaviour
    {
        public LevelStateMachine StateMachine;
        public GraphVisibility Graph;
        public VertexFollower VertexFollower;
        public UserInputHandler UserInputHandler;
        public Player Player;
        public CommandManager CommandManager;

        public static LinksContainer Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
    }
}