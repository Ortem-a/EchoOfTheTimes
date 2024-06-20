using EchoOfTheTimes.Interfaces;
using EchoOfTheTimes.Units;
using System;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.LevelStates
{
    public class MovableByRuleVertex : MonoBehaviour, ISpecialVertex
    {
        private bool _isLinked = false;

        private MovableByRule _movable;
        private Player _player;

        public Action OnEnter => LinkPlayer;

        public Action OnExit => UnlinkPlayer;

        [Inject]
        private void Construct(Player player)
        {
            _player = player;
        }

        private void LinkPlayer()
        {
            if (_player.NextPosition.IsDynamic || _player.StayOnDynamic)
            {
                _isLinked = true;
                _movable.SetPlayer(_player);
            }
        }

        private void UnlinkPlayer()
        {
            if (!_player.NextPosition.IsDynamic && _isLinked)
            {
                _isLinked = false;
                _movable.ClearPlayer();
            }
        }

        public void SetMovable(MovableByRule movableByRule)
        {
            _movable = movableByRule;
        }
    }
}