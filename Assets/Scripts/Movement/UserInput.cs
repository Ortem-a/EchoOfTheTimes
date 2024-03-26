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

        public void Initialize()
        {
            _userInputHandler = GameManager.Instance.UserInputHandler;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Vector3 clickPosition = ScreenToVertex(Input.mousePosition);
                var clickPosition = ScreenToVertex(Input.mousePosition);

                if (clickPosition != null)
                {
                    _userInputHandler.OnMousePressed(clickPosition);
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                _userInputHandler.GoToCheckpoint();
            }
        }

        public Vertex ScreenToVertex(Vector3 screenPosition)
        {
            //Vector3 worldPosition = Vector3.zero;
            Ray ray = _camera.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out RaycastHit hitData, 1000f))
            {
                if (hitData.transform.TryGetComponent(out Vertex vertex))
                {
                    return vertex;
                }
                //worldPosition = hitData.point;
            }

            return null;
        }
    }
}