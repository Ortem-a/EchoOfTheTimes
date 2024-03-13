using DG.Tweening;
using EchoOfTheTimes.Animations;
using EchoOfTheTimes.Commands;
using EchoOfTheTimes.Core;
using EchoOfTheTimes.Interfaces;
using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Movement;
using EchoOfTheTimes.Persistence;
using EchoOfTheTimes.Utils;
using UnityEngine;

namespace EchoOfTheTimes.Units
{
    [RequireComponent(typeof(AnimationManager), typeof(CommandManager))]
    public class Player : MonoBehaviour, IUnit, IBind<PlayerData>
    {
        [field: SerializeField]
        public SerializableGuid Id { get; set; } = SerializableGuid.NewGuid();
        [SerializeField]
        private PlayerData _data;

        public AnimationManager Animations =>
            _animationManager = _animationManager != null ? _animationManager : GetComponent<AnimationManager>();

        [field: SerializeField]
        public float Speed { get; set; } = 5f;

        public bool IsBusy { get; set; } = false;

        public Vertex Position => _graph.GetNearestVertex(transform.position);

        private GraphVisibility _graph;
        private CheckpointManager _checkpointManager;
        private LevelStateMachine _levelStateMachine;

        private AnimationManager _animationManager;

        public void Initialize()
        {
            _graph = GameManager.Instance.Graph;
            _checkpointManager = GameManager.Instance.CheckpointManager;
            _levelStateMachine = GameManager.Instance.StateMachine;
        }

        public void TeleportTo(Vector3 position)
        {
            Debug.Log($"[TeleportTo] {position}");

            this.transform.position = position;
        }

        public void MoveTo(Vector3 destination)
        {
            OnStartExecution();

            //transform.DOLookAt(destination, 0.2f);

            var time = Vector3.Distance(transform.position, destination) / Speed;

            Debug.Log($"[MoveTo] to {destination} | duration {time}");

            var options = transform.DOMove(destination, time)
                .OnComplete(OnCompleteExecution);
        }

        private void OnStartExecution()
        {
            IsBusy = true;

#warning ������
            if (Position.gameObject.TryGetComponent(out LevelStateButton button))
            {
                button.OnRelease?.Invoke();
            }
        }

        private void OnCompleteExecution()
        {
            IsBusy = false;

#warning ����� + ����
            if (Position.gameObject.TryGetComponent(out Checkpoint checkpoint))
            {
                _checkpointManager.OnCheckpointChanged?.Invoke(checkpoint);
            }
            else if (Position.gameObject.TryGetComponent(out Teleportator teleportator))
            {
                teleportator.Teleport();
            }
            else if (Position.gameObject.TryGetComponent(out LevelStateButton button))
            {
                button.OnPress?.Invoke();
            }
        }

        public void Bind(PlayerData data)
        {
#warning �������� �����
            _data = data;
            _data.Id = Id;

            var vertex = _graph.GetNearestVertex(data.Checkpoint);

            _levelStateMachine.LoadState(data.StateId);
            _checkpointManager.OnCheckpointChanged?.Invoke(vertex.gameObject.GetComponent<Checkpoint>());
        }
    }
}