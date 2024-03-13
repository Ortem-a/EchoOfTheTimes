using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Persistence;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace EchoOfTheTimes.Core
{
    public class CheckpointManager : MonoBehaviour
    {
        public Action<Vertex> OnCheckpointChanged;

        public GameData GameData { get; private set; }

        [field: SerializeField]
        public Checkpoint ActiveCheckpoint { get; private set; }

        private void Awake()
        {
            OnCheckpointChanged += UpdateCheckpoint;
        }

        private void OnDestroy()
        {
            OnCheckpointChanged -= UpdateCheckpoint;
        }

        public void UpdateCheckpoint(Vertex vertex)
        {
# warning какая-то хуета. можно лучше.
            if (vertex.gameObject.TryGetComponent(out Checkpoint checkpoint))
            {
                if (!checkpoint.IsVisited)
                {
                    checkpoint.IsVisited = true;

                    GameData.PlayerData.Id = GameManager.Instance.Player.Id;
                    GameData.PlayerData.Checkpoint = vertex.transform.position;
                    GameData.PlayerData.StateId = GameManager.Instance.StateMachine.GetCurrentStateId();
                    //SaveLoadSystem.Instance.SaveGame(GameData);
                }

                ActiveCheckpoint = checkpoint;
                Debug.Log($"[CheckpointManager] Checkpoint changed!");
            }
        }
    }
}