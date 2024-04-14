using EchoOfTheTimes.Core;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Movement
{
    public class UserInput : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        private UserInputHandler _userInputHandler;

        private Vector3 _startTouchPosition;
        private Vector3 _endTouchPosition;
        private Touch _touch;

        [Inject]
        private void Initialize(UserInputHandler inputHandler)
        {
            _userInputHandler = inputHandler;
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

                    if (Mathf.Abs(deltaX) <= 5 && Mathf.Abs(deltaY) <= 5)
                    {
                        var clickPosition = ScreenToVertex(_touch.position);
                        if (clickPosition != null)
                        {
                            _userInputHandler.OnTouched?.Invoke(clickPosition);

                            //Debug.Log("Touch" + ' ' + Mathf.Abs(deltaX) + ' ' + Mathf.Abs(deltaY));
                        }
                    }
                    else
                    {
                        _userInputHandler.OnSwipe?.Invoke(deltaX);

                        //Debug.Log("Swipe" + ' ' + Mathf.Abs(deltaX) + ' ' + Mathf.Abs(deltaY));
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