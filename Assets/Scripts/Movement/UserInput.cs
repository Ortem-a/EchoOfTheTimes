using EchoOfTheTimes.Core;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Movement
{
    public class UserInput : MonoBehaviour
    {
        private Camera _camera;

        private InputMediator _userInputHandler;

        //private float _inputAccuracy = 5f;

        //private Vector3 _startTouchPosition;
        //private Vector3 _endTouchPosition;
        //private Touch _touch;

        private Vector2 _startSwipePosition;
        //private bool _isSwiping = false;
        //private bool _swipeActivated = false;

        //private const float MinSwipeDistance = 70f; // Минимальное расстояние для активации свайпа
        private float _touchStartTime;
        private const float _maxTapTime = 0.2f; // Максимальное время для регистрации тапа
        //private float _lastTapTime = 0f;
        //private const float DoubleTapDelta = 0.5f;
        //private bool _isSuccessfulTap = false;

        [Inject]
        private void Construct(InputMediator inputHandler)
        {
            _userInputHandler = inputHandler;

            _camera = Camera.main;
        }

        // никаких свайпов и дабл тачей, я тут главный и лучше знаю как камера должна быть, игрок ничто, игра всё
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startSwipePosition = Input.mousePosition;
                _touchStartTime = Time.time;
                //_isSwiping = true;
                //_swipeActivated = false;
                //_isSuccessfulTap = false;
            }

            if (Input.GetMouseButtonUp(0))
            {
                float touchDuration = Time.time - _touchStartTime;
                float touchDistance = Vector2.Distance(_startSwipePosition, Input.mousePosition);

                if (touchDuration <= _maxTapTime)
                {
                    Vector3 touchPosition3D = _camera.ScreenToWorldPoint(
                        new Vector3(_startSwipePosition.x, _startSwipePosition.y, _camera.nearClipPlane));

                    if (Physics.Raycast(touchPosition3D, _camera.transform.forward, out RaycastHit hit, Mathf.Infinity))
                    {
                        if (hit.transform.TryGetComponent(out Vertex vertex))
                        {
                            _userInputHandler.OnTouched?.Invoke(vertex);
                            //_isSuccessfulTap = true;
                        }
                    }
                }
            }
        }

        // ляляля тут и свайп детектится и тач, и нет тача по вертексу если свайп начинать с точки вертекса, и дабл тач по пустому месту
        //private void Update() 
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        _startSwipePosition = Input.mousePosition;
        //        _touchStartTime = Time.time;
        //        _isSwiping = true;
        //        _swipeActivated = false;
        //        _isSuccessfulTap = false;
        //    }

        //    if (Input.GetMouseButtonUp(0))
        //    {
        //        float touchDuration = Time.time - _touchStartTime;
        //        float touchDistance = Vector2.Distance(_startSwipePosition, Input.mousePosition);

        //        if (touchDuration <= MaxTapTime && touchDistance < MinSwipeDistance / 2 && !_swipeActivated)
        //        {
        //            Vector3 touchPosition3D = Camera.main.ScreenToWorldPoint(new Vector3(_startSwipePosition.x, _startSwipePosition.y, Camera.main.nearClipPlane));
        //            RaycastHit hit;
        //            if (Physics.Raycast(touchPosition3D, Camera.main.transform.forward, out hit, Mathf.Infinity))
        //            {
        //                EchoOfTheTimes.Core.Vertex vertex = hit.transform.GetComponent<EchoOfTheTimes.Core.Vertex>();
        //                if (vertex != null)
        //                {
        //                    _userInputHandler.OnTouched?.Invoke(vertex);
        //                    _isSuccessfulTap = true;
        //                }
        //            }

        //            if (!_isSuccessfulTap && Time.time - _lastTapTime < DoubleTapDelta)
        //            {
        //                _userInputHandler.OnDoubleTouched?.Invoke(); // Вырезать даблтач, заменив на тач по кнопке для возврата
        //                _lastTapTime = 0f;
        //            }
        //            else
        //            {
        //                _lastTapTime = Time.time;
        //            }
        //        }

        //        _isSwiping = false;
        //        _swipeActivated = false;
        //    }

        //    if (Input.GetMouseButton(0) && _isSwiping) // предполагаем что любое касание = начало свайпа
        //    {
        //        Vector2 currentSwipePosition = Input.mousePosition;

        //        // Считали активированный каким-то чудом свайп
        //        if (!_swipeActivated && (Vector2.Distance(_startSwipePosition, currentSwipePosition) > MinSwipeDistance || Time.time - _touchStartTime > MaxTapTime))
        //        {
        //            _swipeActivated = true;
        //            // _startSwipePosition = currentSwipePosition;
        //        }

        //        if (_swipeActivated)
        //        {

        //            // Свайп в какой величине?
        //            float deltaX = currentSwipePosition.x - _startSwipePosition.x;
        //            // Получаем ширину экрана в дюймах
        //            float screenWidthInches = Screen.width / Screen.dpi;
        //            // Преобразуем deltaX в дюймы
        //            float deltaXInches = deltaX / Screen.dpi;
        //            // Вычисляем угол поворота, так чтобы deltaXInches, равный ширине экрана в дюймах, эквивалентен 180 градусам
        //            float rotationAngle = (deltaXInches / screenWidthInches) * 180;

        //            _userInputHandler.OnSwiped?.Invoke(rotationAngle);

        //            _startSwipePosition = currentSwipePosition; // Я/МЫ еблан? Я считываю микро-отрезочки каждый кадр вместо считывания фул свайпа, и как мне это овкуснивать
        //        }
        //    }
        //}
    }
}