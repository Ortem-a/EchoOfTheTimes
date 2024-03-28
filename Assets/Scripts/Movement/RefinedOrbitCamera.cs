using UnityEngine;

public class RefinedOrbitCamera : MonoBehaviour
{
#warning TODO �����
    // ������� �������� ��� ��������� ��������� ������ ����� ��� � ����
    // �������� ��� ���� � �����
    // ��������� �������� QE
    // ������� �������� ����� QE

    private Camera cam; // ������� ������ �� ��������������� ������
    public Transform centralAxis;
    public Transform player;
    public float minOrthographicSize = 4f; // ����������� ������ ��������������� ��������
    public float maxOrthographicSize = 15f; // ������������ ������ ��������������� ��������
    public float idealOrthographicSize = 10f; // ��������� ������ ��������������� ��������
    public float zoomSpeed = 8f;
    public float returnSpeed = 5f;
    public float tiltAngle = 10f;
    public float inactivityReturnDelay = 5f;
    public float orbitHeight = 10f;
    public float orbitDistance = 10f;
    public float speedHandleRotation = 50f;
    private float followSpeed = 500f;

    private float timeSinceLastZoom = 0f;

    private float timeSinceLastManualRotation = 0f; // ����� � ������� ��������� ������ ����� ��������
    private bool isFollowingPlayer = true; // ������� �� ������ �� �������
    private Vector3 lastPlayerPosition; // ��������� ������� ������ ��� ������������ ��� ��������

    public bool CanMoveCamera = true;
    private Touch _initTouch;
    private float _rotX;
    private float _rotY;
    private Vector3 _originRotation;
    private float _dir = -1;

    private void Awake()
    {
        _originRotation = transform.rotation.eulerAngles;
        _rotX = _originRotation.x;
        _rotY = _originRotation.y;

        // �������� ����� ��������� Camera ����� �������� ��������, ���� �� �� ��� ���������� �������
        if (cam == null)
        {
            cam = GetComponentInChildren<Camera>();
        }

        // ���������, ������� �� ����� ��������� Camera
        if (cam == null)
        {
            Debug.LogError("Camera component not found on " + gameObject.name + " or its children.");
        }
        else
        {
            cam.orthographicSize = idealOrthographicSize;
        }
    }

    private void Update()
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

        if (CanMoveCamera)
        {
            if (Input.touchCount > 0)
            {
                foreach (var touch in Input.touches)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        _initTouch = touch;
                    }
                    else if (touch.phase == TouchPhase.Moved)
                    {
                        float deltaX = _initTouch.position.x - touch.position.x;
                        float deltaY = _initTouch.position.y - touch.position.y;
                        _rotX -= deltaY * Time.deltaTime * speedHandleRotation / 100f * _dir;
                        _rotY += deltaX * Time.deltaTime * speedHandleRotation / 100f * _dir;

                        transform.eulerAngles = new Vector3(_rotX, _rotY, 0f);

                        timeSinceLastManualRotation = 0f;
                        isFollowingPlayer = false;
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        _initTouch = new Touch();
                    }
                }
            }
        }
        else
        {
            // ��������� ����� ��� �������� ������
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
        }

        // �������� �� �������� ������
        if (player.position != lastPlayerPosition)
        {
            isFollowingPlayer = true;
        }

        // �������������� ����������� ������ � ������
        if (timeSinceLastManualRotation >= inactivityReturnDelay && !isFollowingPlayer)
        {
            isFollowingPlayer = true;
        }

        lastPlayerPosition = player.position; // ��������� ��������� ������� ������
    }

    private void LateUpdate()
    {
        if (isFollowingPlayer)
        {
            Vector3 axisToPlayerDirection = (player.position - centralAxis.position).normalized;
            axisToPlayerDirection.y = 0; // ���������� ������������ ���������� ��� ���������� ��������������� �����������

            Vector3 cameraPosition = centralAxis.position + axisToPlayerDirection * orbitDistance;
            cameraPosition.y = centralAxis.position.y + orbitHeight;

            transform.position = Vector3.Lerp(transform.position, cameraPosition, Time.deltaTime * followSpeed);

            // ������������� ���� ������� ������
            Vector3 relativePosition = centralAxis.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePosition);
            Quaternion tilt = Quaternion.Euler(tiltAngle, rotation.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, tilt, Time.deltaTime * followSpeed);
        }
    }

    private void RotateCamera(float direction)
    {
        transform.RotateAround(centralAxis.position, Vector3.up, direction * speedHandleRotation * Time.deltaTime); // �������� ������ ��� Y
        timeSinceLastManualRotation = 0f;
        isFollowingPlayer = false;
    }
}