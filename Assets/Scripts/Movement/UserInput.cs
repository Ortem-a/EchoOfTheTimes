using EchoOfTheTimes.Core;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Movement
{
    public class UserInput : MonoBehaviour
    {
        private Camera _camera;

        private InputMediator _userInputHandler;

        private float _inputAccuracy = 5f;

        private Vector3 _startTouchPosition;
        private Vector3 _endTouchPosition;
        private Touch _touch;

        [Inject]
        private void Construct(InputMediator inputHandler)
        {
            _userInputHandler = inputHandler;

            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                _touch = Input.GetTouch(0);

                if (_touch.phase == TouchPhase.Began)
                {
                    _startTouchPosition = _touch.position;
                }
                else if (_touch.phase == TouchPhase.Moved || _touch.phase == TouchPhase.Ended)
                {
                    _endTouchPosition = _touch.position;

                    float deltaX = _endTouchPosition.x - _startTouchPosition.x;
                    float deltaY = _endTouchPosition.y - _startTouchPosition.y;

                    if (Mathf.Abs(deltaX) <= _inputAccuracy && Mathf.Abs(deltaY) <= _inputAccuracy)
                    {
                        var clickPosition = ScreenToVertex(_touch.position);
                        if (clickPosition != null)
                        {
                            _userInputHandler.OnTouched?.Invoke(clickPosition);
                        }
                    }
                    else
                    {
                        _userInputHandler.OnSwipe?.Invoke(deltaX);
                    }
                }
            }
        }

        private Vertex ScreenToVertex(Vector3 screenPosition)
        {
            Ray ray = _camera.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out RaycastHit hitData, 1000f))
            {
                if (hitData.transform.TryGetComponent(out Vertex vertex))
                {
                    return vertex;
                }
            }

            return null;
        }
    }
}