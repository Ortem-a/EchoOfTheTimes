using System;
using Systems.Movement;

namespace Systems.Units
{
    public interface ITeleportable
    {
        public float TeleportDisappearDuration_sec { get; }
        public float TeleportDuration_sec { get; }
        public bool CanTeleportate { get; set; }

        public void Teleportate(Vertex to, Action onComplete);
    }
}