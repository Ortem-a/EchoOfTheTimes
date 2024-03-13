using EchoOfTheTimes.Core;
using EchoOfTheTimes.LevelStates;
using UnityEngine;

namespace EchoOfTheTimes.Movement
{
    public class UserInput : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        private UserInputHandler _userInputHandler;
        private LevelStateMachine _levelStateMachine;
        private int _input = -1;

        [SerializeField]
        private LevelStateButton _stateButton;
        private bool _isPressed = false; // йняршкей

        public void Initialize()
        {
            _userInputHandler = GameManager.Instance.UserInputHandler;
            _levelStateMachine = GameManager.Instance.StateMachine;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 clickPosition = ScreenToWorldPosition(Input.mousePosition);

                _userInputHandler.OnMousePressed(clickPosition);
            }

            if (Input.GetKeyDown(KeyCode.Alpha0)) _input = 0;
            if (Input.GetKeyDown(KeyCode.Alpha1)) _input = 1;
            if (Input.GetKeyDown(KeyCode.Alpha2)) _input = 2;
            if (Input.GetKeyDown(KeyCode.Alpha3)) _input = 3;
            if (Input.GetKeyDown(KeyCode.Alpha4)) _input = 4;
            if (Input.GetKeyDown(KeyCode.Alpha5)) _input = 5;
            if (Input.GetKeyDown(KeyCode.Alpha6)) _input = 6;
            if (Input.GetKeyDown(KeyCode.Alpha7)) _input = 7;
            if (Input.GetKeyDown(KeyCode.Alpha8)) _input = 8;
            if (Input.GetKeyDown(KeyCode.Alpha9)) _input = 9;

            if (_input != -1)
            {
                _levelStateMachine.ChangeState(_input);

                _input = -1;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!_isPressed)
                {
                    _stateButton.OnPress?.Invoke();
                }
                else
                {
                    _stateButton.OnRelease?.Invoke();
                }

                _isPressed = !_isPressed;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                _userInputHandler.GoToCheckpoint();
            }
        }

        public Vector3 ScreenToWorldPosition(Vector3 screenPosition)
        {
            Vector3 worldPosition = Vector3.zero;
            Ray ray = _camera.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out RaycastHit hitData, 1000f))
            {
                worldPosition = hitData.point;
            }

            return worldPosition;
        }
    }
}