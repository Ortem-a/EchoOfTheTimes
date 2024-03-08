using DG.Tweening;
using EchoOfTheTimes.Animations;
using EchoOfTheTimes.Commands;
using EchoOfTheTimes.Core;
using EchoOfTheTimes.Interfaces;
using EchoOfTheTimes.Persistence;
using EchoOfTheTimes.Utils;
using UnityEngine;

namespace EchoOfTheTimes.Units
{
    [RequireComponent(typeof(AnimationManager), typeof(CommandManager), typeof(UserInputHandler))]
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

        public Vertex StartVertex;

        [SerializeField]
        private GraphVisibility _graph;

        private AnimationManager _animationManager;

        private void Start()
        {
            TeleportTo(StartVertex.transform.position);
        }

        public virtual void TeleportTo(Vector3 position)
        {
            Debug.Log($"[TeleportTo] {position} with speed: {Speed}");

            transform.position = position;
        }

        public virtual void MoveTo(Vector3 destination)
        {
            OnStartExecution();

            transform.DOLookAt(destination, 0.2f);

            var time = Vector3.Distance(transform.position, destination) / Speed;

            Debug.Log($"[MoveTo] to {destination} | duration {time}");

            var options = transform.DOMove(destination, time)
                .OnComplete(OnCompleteExecution);
        }

        private void OnStartExecution()
        {
            IsBusy = true;
        }

        private void OnCompleteExecution()
        {
            IsBusy = false;
        }

        public void Bind(PlayerData data)
        {
            _data = data;
            _data.Id = Id;

            transform.SetPositionAndRotation(_data.Position, _data.Rotation);
            transform.localScale = _data.LocalScale;
        }
    }
}