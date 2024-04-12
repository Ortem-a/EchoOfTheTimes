using EchoOfTheTimes.Core;
using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Units;
using System;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Movement
{
    public class VertexFollower : MonoBehaviour
    {
        private Vertex _vertex;
        private Player _target;

        private bool _isLinked = false;

        public Action OnAcceptLink;

        private void Awake()
        {
            OnAcceptLink += AcceptLink;
        }

        private void OnDestroy()
        {
            OnAcceptLink -= AcceptLink;
        }

        private void Update()
        {
            Follow();
        }

        [Inject]
        private void Initialize(Player player)
        {
            _target = player;
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

        private void LinkPlayer() 
        {
            _vertex = _target.Position;

            _isLinked = true;

            Debug.Log($"[VertexFollower] Link Player with {_vertex.Id}");
        }

        public void Unlink()
        {
            _vertex = null;

            _isLinked = false;

            Debug.Log($"[VertexFollower] Unlink Player");
        }

        private void AcceptLink()
        {
            LinkPlayer();
        }
    }
}