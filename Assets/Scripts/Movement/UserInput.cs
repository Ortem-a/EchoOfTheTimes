using EchoOfTheTimes.Core;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Movement
{
    public class UserInput : MonoBehaviour
    {
        private Camera _camera;
        private InputMediator _userInputMediator;
        private Vector2 _startSwipePosition;
        private float _touchStartTime;
        private const float _maxTapTime = 0.2f; // Максимальное время для регистрации тапа
        private Vector3 _touchPosition;

        [Inject]
        private void Construct(InputMediator inputMediator)
        {
            _userInputMediator = inputMediator;

            _camera = Camera.main;
            _touchPosition = Vector3.forward * _camera.nearClipPlane;
        }

        // никаких свайпов и дабл тачей, я тут главный и лучше знаю как камера должна быть, игрок ничто, игра всё
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

                    _userInputMediator.OnTouchedFirstTime?.Invoke();

                    if (Physics.Raycast(touchPosition3D, _camera.transform.forward, out RaycastHit hit, Mathf.Infinity))
                    {
                        if (hit.transform.TryGetComponent(out Vertex vertex))
                        {
                            _userInputMediator.OnTouched?.Invoke(vertex);
                        }
                    }
                }
            }
        }
    }
}