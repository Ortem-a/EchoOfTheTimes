using EchoOfTheTimes.Interfaces;
using System;
using UnityEngine;

namespace EchoOfTheTimes.Core
{
    public class Checkpoint : MonoBehaviour, ISpecialVertex
    {
        public Vertex Point { get; private set; }

        public Action OnEnter => () => _checkpointManager.OnCheckpointChanged?.Invoke(this);
        public Action OnExit => null;

        private CheckpointManager _checkpointManager;

        private void OnValidate()
        {
            Point = GetComponent<Vertex>();
        }

        public void Initialize()
        {
            _checkpointManager = GameManager.Instance.CheckpointManager;

            Point = Point != null ? Point : GetComponent<Vertex>();
        }
    }
}