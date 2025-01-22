using UnityEngine;
using Zenject;

namespace Systems.Movement
{
    public class Player : MonoBehaviour, IUnit
    {
        public IMovableByPath Movable => throw new System.NotImplementedException();

        public ITeleportable Teleportable => throw new System.NotImplementedException();

        [Inject]
        private void Construct()
        {
            Movable.OnWaypointChanged += HandleNewWaypoint;
        }

        private void OnDestroy()
        {
            Movable.OnWaypointChanged -= HandleNewWaypoint;
        }

        private void HandleNewWaypoint(Vertex waypoint)
        {
            Debug.Log($"[{name}] HANDLE NEW WAYPOINT: {waypoint}");
        }
    }
}