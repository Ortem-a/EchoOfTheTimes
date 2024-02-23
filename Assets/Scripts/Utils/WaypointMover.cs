using UnityEngine;

namespace EchoOfTheTimes.Utils
{
    public class WaypointMover : MonoBehaviour
    {
        [SerializeField] private Waypoints _waypoints; // здесь список контрольных точек
        [SerializeField] private float _moveSpeed = 30f; // скорость перемещения
        private Transform _currentWaypoint; // контрольная точка, к которой объект идет сейчас
                                            //[Range(0.1f, 11f)]
        [SerializeField] private float _distanceTreshold = 11f;

        private BezierMover _curve;
        private Transform _newWP;

        private void Start()
        {
            // установить начальное положение в первый вейпоинт
            _currentWaypoint = _waypoints.GetNextWaypoint(_currentWaypoint);
            transform.position = _currentWaypoint.position;

            // установить следующий вейпоинт
            _currentWaypoint = _waypoints.GetNextWaypoint(_currentWaypoint);

            // повернуться лицом к следующему вейпоинту
            transform.LookAt(_currentWaypoint);
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, _currentWaypoint.position) < _distanceTreshold)
            {
                // установить следующий вейпоинт
                //_currentWaypoint = _waypoints.GetNextWaypoint(_currentWaypoint);
                _newWP = _waypoints.GetNextWaypoint(_currentWaypoint);

                _curve = _currentWaypoint.GetComponent<BezierMover>();

                // повернуться лицом к следующему вейпоинту
                //transform.LookAt(_currentWaypoint);

                transform.position = Bezier.GetPoint(_curve.P0.position, _curve.P1.position, _curve.P2.position, _curve.P3.position, _curve.T);
                transform.rotation = Quaternion.LookRotation(
                    Bezier.GetFirstDerivative(_curve.P0.position, _curve.P1.position, _curve.P2.position, _curve.P3.position, _curve.T)
                    );

                _curve.T += Time.deltaTime;

                if (_curve.T > 1)
                {
                    _currentWaypoint = _newWP;
                    _curve.T = 0;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _currentWaypoint.position, _moveSpeed * Time.deltaTime);
            }
        }
    }
}
