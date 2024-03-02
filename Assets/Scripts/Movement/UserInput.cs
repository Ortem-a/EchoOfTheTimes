using EchoOfTheTimes.Core;
using EchoOfTheTimes.LevelStates;
using UnityEngine;

namespace EchoOfTheTimes.Movement
{
    public class UserInput : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private UserInputHandler _userInputHandler;

        [SerializeField]
        private LevelStateMachine _levelStateMachine;
        private int _input = -1;

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
                Debug.Log($"INPUT: {_input}");

                _levelStateMachine.ChangeState(_input);

                _input = -1;
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