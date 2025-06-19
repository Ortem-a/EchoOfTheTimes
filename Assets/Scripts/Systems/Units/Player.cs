using Systems.Leveling;
using Systems.Movement;
using Systems.Settings;
using UnityEngine;

namespace Systems.Units
{
    public class Player : MonoBehaviour, IUnit
    {
        public IMovableByPath Movable { get; private set; }
        public ITeleportable Teleportable { get; private set; }
        public Transform Transform => transform;
        public bool CanInteractWithStates { get; set; } = true;

        private ISpecialVertex _current;
        private GraphVisibility _graph;

        private void OnDestroy()
        {
            Movable.OnWaypointChanged -= HandleNewWaypoint;
        }

        public IUnit Spawn(UnitSettingsScriptableObject unitSettings, Vertex at, GraphVisibility graph)
        {
            Movable = GetComponent<Movable>();
            Teleportable = GetComponent<Teleportable>();

            _graph = graph;

            Movable.Initialize(unitSettings.MoveSpeed, unitSettings.RotationSpeed, _graph.MaxDistanceToNeighbourVertex);

            Movable.OnWaypointChanged += HandleNewWaypoint;

            transform.position = at.transform.position;
            Movable.CurrentWaypoint = at;

            TryMove(at);

            return this;
        }

        public bool TryMove(Vertex to)
        {
            Vertex start = Movable.NextWaypoint != null ? Movable.NextWaypoint : Movable.CurrentWaypoint;

            var path = _graph.GetPathBFS(start, to);

            if (path.Count == 0)
            {
                return false;
            }

            Movable.MoveBy(path);
            return true;
        }

        private void HandleNewWaypoint(Vertex waypoint)
        {
            if (waypoint.TryGetComponent<ISpecialVertex>(out var specialVertex))
            {
                // if previous waypoint was ISpecialVertex and current waypoint also special vertex
                if (_current != null)
                {
                    _current.OnExit(this);
                    _current = null;
                }
                // if previous waypoint not ISpecialVertex and current is
                else
                {
                    specialVertex.OnEnter(this);
                    _current = specialVertex;
                }
            }
            else
            {
                // if current waypoint not ISpecialVertex
                if (_current != null)
                {
                    _current.OnExit(this);
                    _current = null;
                }
            }
        }
    }
}