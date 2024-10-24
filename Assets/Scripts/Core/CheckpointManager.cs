using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Persistence;
using EchoOfTheTimes.Units;
using System;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Core
{
    public class CheckpointManager : MonoBehaviour
    {
        public Action<Checkpoint> OnCheckpointChanged;

        public PlayerData StartPlayerData;
        public Checkpoint StartCheckpoint;

        public PlayerData PlayerData;
        public Checkpoint ActiveCheckpoint;

        private Player _player;
        private LevelStateMachine _stateMachine;

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

            _player.transform.position = StartCheckpoint.transform.position;
            _stateMachine.ChangeStateImmediate(StartPlayerData.StateId);
        }

        private void OnDestroy()
        {
            OnCheckpointChanged -= UpdateCheckpoint;
        }

        [Inject]
        private void Construct(Player player, LevelStateMachine stateMachine)
        {
            _player = player;
            _stateMachine = stateMachine;

            ActiveCheckpoint = StartCheckpoint;

            PlayerData = new PlayerData()
            {
                StateId = StartPlayerData.StateId,
                Checkpoint = StartPlayerData.Checkpoint
            };
        }

        private void UpdateCheckpoint(Checkpoint checkpoint)
        {
            PlayerData.StateId = _stateMachine.GetCurrentStateId();
            PlayerData.Checkpoint = checkpoint.transform.position;

            ActiveCheckpoint = checkpoint;
            Debug.Log($"[CheckpointManager] Checkpoint changed! | PlayerData {PlayerData} | ActiveCheckpoint {ActiveCheckpoint}");
        }

        public void AcceptActiveCheckpointToScene()
        {
            Debug.Log($"[CheckpointManager] Accept Active Checkpoint To Scene '{PlayerData}'");

            _player.Teleportate(PlayerData.Checkpoint, 0.1f);
            _stateMachine.LoadState(PlayerData.StateId);
        }
    }
}