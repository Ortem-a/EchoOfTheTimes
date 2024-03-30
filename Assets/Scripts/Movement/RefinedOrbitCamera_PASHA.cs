using EchoOfTheTimes.Training;
using System.Security.Cryptography;
using UnityEngine;

namespace EchoOfTheTimes.Movement
{
    public class RefinedOrbitCamera_PASHA : MonoBehaviour
    {
        private Camera cam; 
        public Transform centralAxis;
        public Transform player;
        public float minOrthographicSize = 4f; 
        public float maxOrthographicSize = 15f; 
        public float idealOrthographicSize = 10f; 
        public float zoomSpeed = 8f;
        public float returnSpeed = 5f;
        public float tiltAngle = 10f;
        public float inactivityReturnDelay = 5f;
        public float orbitHeight = 10f;
        public float orbitDistance = 10f;
        public float speedHandleRotation = 50f;
        private float followSpeed = 500f;

        private float timeSinceLastZoom = 0f;

        private float timeSinceLastManualRotation = 0f; 
        private bool isFollowingPlayer = true; 
        private Vector3 lastPlayerPosition; 

        void Start()
        {
            if (cam == null)
            {
                cam = GetComponentInChildren<Camera>();
            }

            if (cam == null)
            {
                Debug.LogError("Camera component not found on " + gameObject.name + " or its children.");
            }
            else
            {
                cam.orthographicSize = idealOrthographicSize;
            }
        }

        void Update()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f)
            {
                cam.orthographicSize -= scroll * zoomSpeed;
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minOrthographicSize, maxOrthographicSize);
                timeSinceLastZoom = 0f;
            }
            else
            {
                timeSinceLastZoom += Time.deltaTime;
                if (timeSinceLastZoom >= inactivityReturnDelay && cam.orthographicSize != idealOrthographicSize)
                {
                    cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, idealOrthographicSize, Time.deltaTime * returnSpeed);
                }
            }

            if (Input.GetKey(KeyCode.Q))
            {
                RotateCamera(-1);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                RotateCamera(1);
            }
            else
            {
                timeSinceLastManualRotation += Time.deltaTime;
            }

            if (player.position != lastPlayerPosition)
            {
                isFollowingPlayer = true;
            }

            if (timeSinceLastManualRotation >= inactivityReturnDelay && !isFollowingPlayer)
            {
                isFollowingPlayer = true;
            }

            lastPlayerPosition = player.position;
        }

        void LateUpdate()
        {
            if (isFollowingPlayer)
            {
                Vector3 axisToPlayerDirection = (player.position - centralAxis.position).normalized;
                axisToPlayerDirection.y = 0;

                Vector3 cameraPosition = centralAxis.position + axisToPlayerDirection * orbitDistance;
                cameraPosition.y = centralAxis.position.y + orbitHeight;

                transform.position = Vector3.Lerp(transform.position, cameraPosition, Time.deltaTime * followSpeed);

                Vector3 relativePosition = centralAxis.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(relativePosition);
                Quaternion tilt = Quaternion.Euler(tiltAngle, rotation.eulerAngles.y, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation, tilt, Time.deltaTime * followSpeed);
            }
        }

        private void RotateCamera(float direction)
        {
            transform.RotateAround(centralAxis.position, Vector3.up, direction * Time.deltaTime);
            //transform.RotateAround(centralAxis.position, Vector3.up, direction * speedHandleRotation * Time.deltaTime);
            timeSinceLastManualRotation = 0f;
            isFollowingPlayer = false;
        }

        public bool CanRotateCamera { get; set; } = true;
        public RotationTrainerActivator RTA;
        public void HandleSwipe(float deltaX)
        {
            if (CanRotateCamera)
            {
                HideRTA();

                RotateCamera(deltaX);
            }
        }

        private bool _flagOnce = true;
        private void HideRTA()
        {
            if (_flagOnce)
            {
                RTA.Hide();
                _flagOnce = false;
            }
        }
    }
}