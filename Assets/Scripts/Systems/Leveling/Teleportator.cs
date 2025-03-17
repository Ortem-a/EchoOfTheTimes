using Systems.Movement;
using Systems.Units;
using UnityEngine;

namespace Systems.Leveling
{
    public class Teleportator : MonoBehaviour, ISpecialVertex
    {
        [field: SerializeField]
        public Teleportator Destination { get; set; }
        public SpecialVertexType Type { get; private set; } = SpecialVertexType.Teleportator;

        public Vertex Vertex
        {
            get
            {
                if (_vertex == null)
                {
                    _vertex = GetComponent<Vertex>();
                }

                return _vertex;
            }
        }

        private Vertex _vertex;

        public void OnEnter(IUnit unit)
        {
            Teleportate(unit);
        }

        public void OnExit(IUnit unit)
        {
            throw new System.NotImplementedException();
        }

        private void Teleportate(IUnit unit)
        {
            Debug.Log($"[Teleportator] Teleport to {Destination}");

            unit.Movable.StopImmediate();

            OnStartTeleportation();

            unit.Movable.CurrentWaypoint = Destination.Vertex;

            unit.Teleportable.Teleportate(Destination.Vertex,
                () =>
                {
                    OnCompleteTeleportation();
                    unit.Movable.SetParent(Destination.Vertex);
                });
        }

        private void OnStartTeleportation()
        {
            // sound management
            // ui management
        }

        private void OnCompleteTeleportation()
        {
            // sound management
            // ui management
        }
    }
}