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

            IsPressed = true;

            Debug.Log($"[LevelButton] {name} pressed! IsPressed: {IsPressed}");

            _graph.ResetVertices();

            _player.StopAndLink(onComplete: () =>
            {
                for (int i = 0; i < _maxMovables; i++)
                {
                    Movables[i].Move(onComplete: ExecutePostActions);
                }
            });   
        }

        private void ExecutePostActions()
        {
            _counter++;

            if (_counter == _maxMovables)
            {
                _graph.Load();

                _player.ForceUnlink();
            }
        }
    }
}