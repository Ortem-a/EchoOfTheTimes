using UnityEngine;

namespace EchoOfTheTimes.Utils
{
    public class WaypointMover : MonoBehaviour
    {
        [SerializeField] private Waypoints _waypoints; // ����� ������ ����������� �����
        [SerializeField] private float _moveSpeed = 30f; // �������� �����������
        private Transform _currentWaypoint; // ����������� �����, � ������� ������ ���� ������
                                            //[Range(0.1f, 11f)]
        [SerializeField] private float _distanceTreshold = 11f;

        private BezierMover _curve;
        private Transform _newWP;

        private void Start()
        {
            // ���������� ��������� ��������� � ������ ��������
            _currentWaypoint = _waypoints.GetNextWaypoint(_currentWaypoint);
            transform.position = _currentWaypoint.position;

            // ���������� ��������� ��������
            _currentWaypoint = _waypoints.GetNextWaypoint(_currentWaypoint);

            // ����������� ����� � ���������� ���������
            transform.LookAt(_currentWaypoint);
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, _currentWaypoint.position) < _distanceTreshold)
            {
                // ���������� ��������� ��������
                //_currentWaypoint = _waypoints.GetNextWaypoint(_currentWaypoint);
                _newWP = _waypoints.GetNextWaypoint(_currentWaypoint);

                _curve = _currentWaypoint.GetComponent<BezierMover>();

                // ����������� ����� � ���������� ���������
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
