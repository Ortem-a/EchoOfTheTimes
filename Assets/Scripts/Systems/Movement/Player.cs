using Systems.Leveling;
using UnityEngine;
using Zenject;

namespace Systems.Movement
{
    public class Player : MonoBehaviour, IUnit
    {
        public IMovableByPath Movable { get; private set; }

        public ITeleportable Teleportable { get; private set; }

        private bool _canTeleportate = false;

        private GraphVisibility _graph;

        [Inject]
        private void Construct(GraphVisibility graph)
        {
            Movable = GetComponent<Movable>();
            Teleportable = GetComponent<Teleportable>();

            _graph = graph;

            Movable.OnWaypointChanged += HandleNewWaypoint;
        }

        private void OnDestroy()
        {
            Movable.OnWaypointChanged -= HandleNewWaypoint;
        }

        public void Spawn(Vertex at)
        {
            transform.position = at.transform.position;
            Movable.CurrentWaypoint = at;

            Move(at);
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