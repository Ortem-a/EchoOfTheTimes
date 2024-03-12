using EchoOfTheTimes.Core;
using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Units;
using UnityEngine;

namespace EchoOfTheTimes.Movement
{
    public class VertexFollower : MonoBehaviour
    {
        private Vertex _vertex;
        private Transform _target;

        private bool _isLinked = false;

        private void LateUpdate()
        {
            Follow();
        }

        private void Follow()
        {
            if (_isLinked)
            {
                if (_target.position != _vertex.transform.position)
                {
                    _target.position = _vertex.transform.position;
                }
            }
        }

        public void LinkDefault()
        {
            if (TryGetComponent(out Player player))
            {
                Link(player.transform, player.Position);
            }
            else
            {
                Debug.LogWarning($"Can't link this '{name}' by defaults!");
            }
        }

        public void Link(Transform target, Vertex vertex) 
        {
            _target = target;
            _vertex = vertex;

            _isLinked = true;
        }

        public void Unlink()
        {
            _target = null;
            _vertex = null;

            _isLinked = false;
        }
    }
}