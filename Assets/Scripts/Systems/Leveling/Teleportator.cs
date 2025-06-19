using Systems.Units;
using UnityEngine;

namespace Systems.Leveling
{
    public sealed class Teleportator : AbstractSpecialVertex
    {
        [field: SerializeField]
        public Teleportator Destination { get; set; }

        public override void OnEnter(IUnit unit)
        {
            Teleportate(unit);
        }

        public override void OnExit(IUnit unit) { }

        private void Teleportate(IUnit unit)
        {
            Debug.Log($"[Teleportator] Teleport to {Destination}");

            unit.Movable.StopImmediate();

            OnStartTeleportation();

            unit.Movable.CurrentWaypoint = Destination;

            unit.Teleportable.Teleportate(Destination,
                () =>
                {
                    OnCompleteTeleportation();
                    unit.Movable.SetParent(Destination);
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