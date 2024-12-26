using Systems.Movement;
using UnityEngine;
using Zenject;

namespace Systems
{
    public class TestUserInput : MonoBehaviour
    {
        private Camera _camera;
        private TestInputAdapter _inputAdapter;
        private Vector2 _startSwipePosition;
        private float _touchStartTime;
        private const float _maxTapTime = 0.2f;
        private Vector3 _touchPosition;

        [Inject]
        private void Construct(TestInputAdapter inputAdapter)
        {
            _camera = Camera.main;
            _touchPosition = Vector3.forward * _camera.nearClipPlane;
            _inputAdapter = inputAdapter;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startSwipePosition = Input.mousePosition;
                _touchStartTime = Time.time;
            }

            if (Input.GetMouseButtonUp(0))
            {
                float touchDuration = Time.time - _touchStartTime;

                if (touchDuration <= _maxTapTime)
                {
                    _touchPosition.x = _startSwipePosition.x;
                    _touchPosition.y = _startSwipePosition.y;
                    Vector3 touchPosition3D = _camera.ScreenToWorldPoint(_touchPosition);

                    if (Physics.Raycast(touchPosition3D, _camera.transform.forward, out RaycastHit hit, Mathf.Infinity))
                    {
                        if (hit.transform.TryGetComponent(out Vertex vertex))
                        {
                            _inputAdapter.HandleTouch(vertex);
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _inputAdapter.StopPlayer();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _inputAdapter.SwitchState(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _inputAdapter.SwitchState(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _inputAdapter.SwitchState(2);
            }
        }
    }
}