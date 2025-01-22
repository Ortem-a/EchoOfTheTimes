using System;

namespace Systems.Movement
{
    public interface ITeleportable
    {
        public float TeleportDisappearDuration_sec { get; }
        public float TeleportDuration_sec { get; }

        public void Teleportate(Vertex to, Action onComplete);
    }
}