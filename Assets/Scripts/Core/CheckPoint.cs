using EchoOfTheTimes.Interfaces;
using System;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Core
{
    public class Checkpoint : MonoBehaviour, ISpecialVertex
    {
        public Vertex Point { get; private set; }

        public Action OnEnter => null;// () => _checkpointManager.OnCheckpointChanged?.Invoke(this);
        public Action OnExit => null;

        private CheckpointManager _checkpointManager;

        private void OnValidate()
        {
            Point = GetComponent<Vertex>();
        }

        [Inject]
        private void Construct(CheckpointManager checkpointManager)
        {
            _checkpointManager = checkpointManager;

            Point = Point != null ? Point : GetComponent<Vertex>();
        }
    }
}