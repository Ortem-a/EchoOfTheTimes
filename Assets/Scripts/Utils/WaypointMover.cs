using UnityEngine;

namespace EchoOfTheTimes.Utils
{
    public class WaypointMover : MonoBehaviour
    {
        [SerializeField]
        private WaypointController _waypointController;

        [SerializeField]
        private float _moveSpeed = 30f;

        private Waypoint _currentWaypoint;

        [SerializeField] private float _distanceTreshold = 11f;

        private BezierCurve _curve;
        private Waypoint _newWP;

        private void Start()
        {
            if (_waypointController.TryGetNextWaypoint(out _currentWaypoint))
            {
                transform.position = _currentWaypoint.Point;
            }

            if (_waypointController.TryGetNextWaypoint(out _currentWaypoint))
            {
                transform.LookAt(_currentWaypoint.transform);
            }
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, _currentWaypoint.Point) < _distanceTreshold)
            {
                if (_waypointController.TryGetNextWaypoint(out _newWP))
                {
                    _currentWaypoint = _newWP;
                    transform.LookAt(_currentWaypoint.Point);

                    //_curve = _currentWaypoint.Curve;

                    //transform.SetPositionAndRotation(
                    //    Bezier.GetPoint(_curve.P0.position, _curve.P1.position, _curve.P2.position, _curve.P3.position, _curve.T),
                    //    Quaternion.LookRotation(
                    //        Bezier.GetFirstDerivative(_curve.P0.position, _curve.P1.position, _curve.P2.position, _curve.P3.position, _curve.T)
                    //    ));

                    //_curve.T += Time.deltaTime;

                    //if (_curve.T > 1)
                    //{
                    //    _currentWaypoint = _newWP;
                    //    _curve.T = 0;
                    //}
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _currentWaypoint.Point, _moveSpeed * Time.deltaTime);
            }
        }
    }
}
