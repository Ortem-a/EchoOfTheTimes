using EchoOfTheTimes.Core;
using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Units;
using System;
using UnityEngine;

namespace EchoOfTheTimes.Movement
{
    public class VertexFollower : MonoBehaviour
    {
        private Vertex _vertex;
        private Player _target;

        private bool _isLinked = false;

        public Action<bool> OnAcceptLink;

        private void Awake()
        {
            OnAcceptLink += AcceptLink;
        }

        private void OnDestroy()
        {
            OnAcceptLink -= AcceptLink;
        }

        private void LateUpdate()
        {
            Follow();
        }

        public void Initialize()
        {
            _target = GameManager.Instance.Player;
        }

        private void Follow()
        {
            if (_isLinked)
            {
                if (_target.transform.position != _vertex.transform.position)
                {
                    _target.transform.position = _vertex.transform.position;
                }
            }
        }

        private void LinkPlayer(LevelStateMachine.StateMachineCallback callback = null) 
        {
            _vertex = _target.Position;

            _isLinked = true;
        }

        public void Unlink(LevelStateMachine.StateMachineCallback callback = null)
        {
            _vertex = null;

            _isLinked = false;
        }

        private void AcceptLink(bool accept = true)
        {
            LinkPlayer();

            //_isLinked = accept;
        }
    }
}