using EchoOfTheTimes.EditorTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Utils
{
    public class WaypointController : MonoBehaviour
    {
        private Waypoint[] _waypoints;

        private int _currentWaypointIndex;

        [Space]
        [InspectorButton(nameof(UpdateWaypoints))]
        public bool IsUpdateWaypoints;

        public void UpdateWaypoints()
        {
            _waypoints = GetComponentsInChildren<Waypoint>();

            if (_waypoints == null || _waypoints.Length == 0 ) 
            {
                Debug.LogWarning("There is no waypoints!");
            }
            else
            {
                Debug.Log($"Successfully updated waypoints! {_waypoints.Length} waypoints founded");
            }
        }

        private void OnDrawGizmos()
        {
            if (_waypoints != null)
            {
                Gizmos.color = Color.red;
                for (int i = 0; i < _waypoints.Length - 1; i++)
                {
                    Gizmos.DrawLine(_waypoints[i].Point, _waypoints[i + 1].Point);
                }
                Gizmos.color = Color.white;
            }
        }

        public bool TryGetNextWaypoint(out Waypoint waypoint)
        {
            waypoint = null;

            if (_waypoints == null || _waypoints.Length == 0)
            {
                UpdateWaypoints();

                return false;
            }

            if (_currentWaypointIndex < _waypoints.Length)
            {
                waypoint = _waypoints[_currentWaypointIndex];

                _currentWaypointIndex++;

                return true;
            }

            return false;
        }
    }
}