using Systems.Leveling;
using Systems.Movement;
using UnityEngine;

namespace Systems.Units
{
    public class Player : MonoBehaviour, IUnit
    {
        public IMovableByPath Movable { get; private set; }

        public ITeleportable Teleportable { get; private set; }

        public Transform Transform => transform;

        private bool _canTeleportate = false;

        private GraphVisibility _graph;

        private void OnDestroy()
        {
            Movable.OnWaypointChanged -= HandleNewWaypoint;
        }

        public IUnit Spawn(Vertex at, GraphVisibility graph)
        {
            Movable = GetComponent<Movable>();
            Teleportable = GetComponent<Teleportable>();

            _graph = graph;

            Movable.OnWaypointChanged += HandleNewWaypoint;

            transform.position = at.transform.position;
            Movable.CurrentWaypoint = at;

            Move(at);

            return this;
        }

        public void Move(Vertex to)
        {
            Vertex start = Movable.NextWaypoint != null ? Movable.NextWaypoint : Movable.CurrentWaypoint;

            var path = _graph.GetPathBFS(start, to);
            Movable.MoveBy(path);
        }

        private void HandleNewWaypoint(Vertex waypoint)
        {
            if (waypoint.TryGetComponent<ISpecialVertex>(out var specialVertex))
            {
                switch (specialVertex.Type)
                {
                    case SpecialVertexType.Button:
                        specialVertex.OnEnter(this);
                        break;
                    case SpecialVertexType.Teleportator:
                        _canTeleportate = !_canTeleportate;
                        if (_canTeleportate)
                        {
                            specialVertex.OnEnter(this);
                        }
                        break;
                    default:
                        throw new System.NotImplementedException(specialVertex.Type.ToString());
                }
            }
        }
    }
}