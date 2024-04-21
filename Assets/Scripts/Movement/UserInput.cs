using EchoOfTheTimes.Core;
using UnityEngine;
using UnityEngine.UIElements;
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



        private Vector2 _startSwipePosition;
        private bool _isSwiping = false;
        private bool _swipeActivated = false;

        private const float MinSwipeDistance = 70f; // Минимальное расстояние для активации свайпа
        private float _touchStartTime;
        private const float MaxTapTime = 0.2f; // Максимальное время для регистрации тапа
        private float _lastTapTime = 0f;
        private const float DoubleTapDelta = 0.5f;
        private bool _isSuccessfulTap = false;

        [Inject]
        private void Construct(InputMediator inputHandler)
        {
            _userInputHandler = inputHandler;

            _camera = Camera.main;
        }

        //private void Update()
        //{
        //    if (Input.touchCount > 0)
        //    {
        //        _touch = Input.GetTouch(0);

        //        if (_touch.phase == TouchPhase.Began)
        //        {
        //            _startTouchPosition = _touch.position;
        //        }
        //        else if (_touch.phase == TouchPhase.Moved || _touch.phase == TouchPhase.Ended)
        //        {
        //            _endTouchPosition = _touch.position;

        //            float deltaX = _endTouchPosition.x - _startTouchPosition.x;
        //            float deltaY = _endTouchPosition.y - _startTouchPosition.y;

        //            if (Mathf.Abs(deltaX) <= _inputAccuracy && Mathf.Abs(deltaY) <= _inputAccuracy)
        //            {
        //                var clickPosition = ScreenToVertex(_touch.position);
        //                if (clickPosition != null)
        //                {
        //                    _userInputHandler.OnTouched?.Invoke(clickPosition);
        //                }
        //            }
        //            else
        //            {
        //                _userInputHandler.OnSwipe?.Invoke(deltaX);
        //            }
        //        }
        //    }
        //}



        private void Update() // ляляля тут и свайп детектится и тач, и нет тача по вертексу если свайп начинать с точки вертекса, и дабл тач по пустому месту
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startSwipePosition = Input.mousePosition;
                _touchStartTime = Time.time;
                _isSwiping = true;
                _swipeActivated = false;
                _isSuccessfulTap = false;
            }

            if (Input.GetMouseButtonUp(0))
            {
                float touchDuration = Time.time - _touchStartTime;
                float touchDistance = Vector2.Distance(_startSwipePosition, Input.mousePosition);

                if (touchDuration <= MaxTapTime && touchDistance < MinSwipeDistance / 2 && !_swipeActivated)
                {
                    Vector3 touchPosition3D = Camera.main.ScreenToWorldPoint(new Vector3(_startSwipePosition.x, _startSwipePosition.y, Camera.main.nearClipPlane));
                    RaycastHit hit;
                    if (Physics.Raycast(touchPosition3D, Camera.main.transform.forward, out hit, Mathf.Infinity))
                    {
                        EchoOfTheTimes.Core.Vertex vertex = hit.transform.GetComponent<EchoOfTheTimes.Core.Vertex>();
                        if (vertex != null)
                        {
                            _userInputHandler.OnTouched?.Invoke(vertex);
                            _isSuccessfulTap = true;
                        }
                    }

                    if (!_isSuccessfulTap && Time.time - _lastTapTime < DoubleTapDelta)
                    {
                        _userInputHandler.OnDoubleTouched?.Invoke();
                        _lastTapTime = 0f;
                    }
                    else
                    {
                        _lastTapTime = Time.time;
                    }
                }

                _isSwiping = false;
                _swipeActivated = false;
            }

            if (Input.GetMouseButton(0) && _isSwiping) // предполагаем что любое касание = начало свайпа
            {
                Vector2 currentSwipePosition = Input.mousePosition;

                if (!_swipeActivated && (Vector2.Distance(_startSwipePosition, currentSwipePosition) > MinSwipeDistance || Time.time - _touchStartTime > MaxTapTime))
                {
                    _swipeActivated = true;
                    _startSwipePosition = currentSwipePosition;
                }

                if (_swipeActivated)
                {
                    float deltaX = (currentSwipePosition.x - _startSwipePosition.x) / Screen.width * 50000;
                    _userInputHandler.OnSwiped?.Invoke(deltaX);
                    _startSwipePosition = currentSwipePosition;
                }
            }
        }


        //private EchoOfTheTimes.Core.Vertex ScreenToVertex(Vector3 screenPosition)
        //{
        //    Ray ray = _camera.ScreenPointToRay(screenPosition);
        //    if (Physics.Raycast(ray, out RaycastHit hitData, 1000f))
        //    {
        //        if (hitData.transform.TryGetComponent(out EchoOfTheTimes.Core.Vertex vertex))
        //        {
        //            return vertex;
        //        }
        //    }

        //    return null;
        //}
    }
}