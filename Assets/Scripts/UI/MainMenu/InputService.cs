using UnityEngine;

namespace EchoOfTheTimes.UI.MainMenu
{
    public class InputService : MonoBehaviour
    {
        private Camera _camera;
        private Vector3 _touchPosition;
        private Vector2 _startSwipePosition;
        private float _touchStartTime;
        private const float _maxTapTime = 0.2f;

        private void Awake()
        {
            _camera = Camera.main;
            _touchPosition = Vector3.forward * _camera.nearClipPlane;
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
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

                Vector3 mousePosition = Input.mousePosition;

                float touchDuration = Time.time - _touchStartTime;
                float touchDistance = Vector2.Distance(_startSwipePosition, mousePosition);

                if (touchDuration <= _maxTapTime)
                {
                    _touchPosition.x = _startSwipePosition.x;
                    _touchPosition.y = _startSwipePosition.y;
                    Vector3 touchPosition3D = _camera.ScreenToWorldPoint(_touchPosition);

                    if (Physics.Raycast(touchPosition3D, _camera.transform.forward, out RaycastHit hit, Mathf.Infinity))
                    {
                        if (hit.transform.TryGetComponent(out Segment segment))
                        {
                            segment.HandleTouch();
                        }
                        else if (hit.transform.TryGetComponent(out Column column))
                        {
                            column.Raise();
                        }
                    }
                }
            }
        }
    }
}