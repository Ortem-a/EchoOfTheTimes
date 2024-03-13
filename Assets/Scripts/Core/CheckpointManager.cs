using System;
using UnityEngine;

namespace EchoOfTheTimes.Core
{
    public class CheckpointManager : MonoBehaviour
    {
        public Action<Checkpoint> OnCheckpointChanged;

        public Checkpoint[] Checkpoints;

        [field: SerializeField]
        public Checkpoint ActiveCheckpoint { get; private set; }

        private void Awake()
        {
            OnCheckpointChanged += ChangeCheckpoint;
        }

        private void OnDestroy()
        {
            OnCheckpointChanged -= ChangeCheckpoint;
        }

        public void ResetCheckpoints()
        {
            Checkpoints = new Checkpoint[0];

            var checkpoints = FindObjectsOfType<Checkpoint>();

            Checkpoints = checkpoints;
        }

        private void ChangeCheckpoint(Checkpoint checkpoint)
        {
            ActiveCheckpoint = checkpoint;

            Debug.Log($"[CheckpointManager] Checkpoint changed!");
        }
    }
}