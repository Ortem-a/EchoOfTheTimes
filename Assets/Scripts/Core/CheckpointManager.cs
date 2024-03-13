using EchoOfTheTimes.LevelStates;
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

        private void Awake()
        {
            OnCheckpointChanged += UpdateCheckpoint;

            ActiveCheckpoint = StartCheckpoint;
            PlayerData.StateId = StartPlayerData.StateId;
            PlayerData.Checkpoint = StartPlayerData.Checkpoint;
        }

        public void Initialize()
        {
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
            Debug.Log($"[CheckpointManager] Checkpoint changed!");
        }

        private void AcceptActiveCheckpointToScene()
        {
            GameManager.Instance.StateMachine.LoadState(PlayerData.StateId);
            GameManager.Instance.Player.TeleportTo(PlayerData.Checkpoint);
        }
    }
}