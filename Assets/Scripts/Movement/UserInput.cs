using EchoOfTheTimes.Core;
using UnityEngine;

namespace EchoOfTheTimes.Movement
{
    public class UserInput : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private UserInputHandler _userInputHandler;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 clickPosition = ScreenToWorldPosition(Input.mousePosition);

                _userInputHandler.OnMousePressed(clickPosition);
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