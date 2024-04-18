using EchoOfTheTimes.Core;
using EchoOfTheTimes.Interfaces;
using EchoOfTheTimes.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.LevelStates
{
    public class LevelButton : MonoBehaviour, ISpecialVertex
    {
        public List<MovableByButton> Movables;

        public bool IsPressed { get; private set; } = false;

        public Action OnEnter => Press;
        public Action OnExit => null;

        private GraphVisibility _graph;
        private Player _player;
        private int _maxMovables;
        private int _counter;

        [Inject]
        private void Construct(GraphVisibility graph, Player player)
        {
            _graph = graph;
            _player = player;

            _maxMovables = Movables != null ? Movables.Count : 0;
            _counter = 0;
        }

        private void Press()
        {
            if (IsPressed) return;

            _graph.ResetVertices();

            IsPressed = true;

            Debug.Log($"[LevelButton] {name} pressed! IsPressed: {IsPressed}");

            foreach (var movable in Movables)
            {
                movable.Move(onComplete: ExecutePostActions);
            }

            _player.Stop(null);
        }

        private void ExecutePostActions()
        {
            _counter++;

            if (_counter == _maxMovables)
            {
                _graph.Load();
            }
        }
    }
}