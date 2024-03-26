using EchoOfTheTimes.Persistence;
using System;
using UnityEngine;

namespace EchoOfTheTimes.Core
{
    public class CheckpointManager : MonoBehaviour
    {
        public Action<Checkpoint> OnCheckpointChanged;

        public PlayerData StartPlayerData;
        public Checkpoint StartCheckpoint;

        public PlayerData PlayerData;
        public Checkpoint ActiveCheckpoint;

        private void OnValidate()
        {
            if (StartCheckpoint != null) 
            {
                StartPlayerData.Checkpoint = StartCheckpoint.transform.position;
            }
        }

        private void Awake()
        {
            OnCheckpointChanged += UpdateCheckpoint;
        }

        public void Initialize()
        {
            ActiveCheckpoint = StartCheckpoint;

            PlayerData = new PlayerData()
            {
                Id = StartPlayerData.Id,
                StateId = StartPlayerData.StateId,
                Checkpoint = StartPlayerData.Checkpoint
            };

            AcceptActiveCheckpointToScene();
        }

        private void OnDestroy()
        {
            OnCheckpointChanged -= UpdateCheckpoint;
        }

        private void UpdateCheckpoint(Checkpoint checkpoint)
        {
#warning какая-то хуета. можно лучше.

            PlayerData.Id = GameManager.Instance.Player.Id;
            PlayerData.Checkpoint = checkpoint.transform.position;
            PlayerData.StateId = GameManager.Instance.StateMachine.GetCurrentStateId();
            //SaveLoadSystem.Instance.SaveGame(GameData);

            ActiveCheckpoint = checkpoint;
            Debug.Log($"[CheckpointManager] Checkpoint changed! | PlayerData {PlayerData} | ActiveCheckpoint {ActiveCheckpoint}");
        }

        public void AcceptActiveCheckpointToScene()
        {
            Debug.Log($"[CheckpointManager] Accept Active Checkpoint To Scene '{PlayerData}'");

            GameManager.Instance.Player.Teleportate(PlayerData.Checkpoint, 0.1f);
            GameManager.Instance.StateMachine.LoadState(PlayerData.StateId);
        }
    }
}