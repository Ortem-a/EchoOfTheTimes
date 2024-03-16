using UnityEngine;

public class RefinedOrbitCamera : MonoBehaviour
{
#warning TODO нахуй
    // сделать проверку что идеальное состояние всегда между мин и макс
    // добавить тул типы к полям
    // исправить повороты QE
    // плавные возвраты после QE

    private Camera cam; // Добавим ссылку на ортографическую камеру
    public Transform centralAxis;
    public Transform player;
    public float minOrthographicSize = 4f; // Минимальный размер ортографической проекции
    public float maxOrthographicSize = 15f; // Максимальный размер ортографической проекции
    public float idealOrthographicSize = 10f; // Идеальный размер ортографической проекции
    public float zoomSpeed = 8f;
    public float returnSpeed = 5f;
    public float tiltAngle = 10f;
    public float inactivityReturnDelay = 5f;
    public float orbitHeight = 10f;
    public float orbitDistance = 10f;
    public float speedHandleRotation = 50f;
    private float followSpeed = 500f;

    private float timeSinceLastZoom = 0f;

    private float timeSinceLastManualRotation = 0f; // Время с момента последней ручной смены поворота
    private bool isFollowingPlayer = true; // Следует ли камера за игроком
    private Vector3 lastPlayerPosition; // Последняя позиция игрока для отслеживания его движения

    void Start()
    {
        // Пытаемся найти компонент Camera среди дочерних объектов, если он не был установлен вручную
        if (cam == null)
        {
            cam = GetComponentInChildren<Camera>();
        }

        // Проверяем, удалось ли найти компонент Camera
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

        // Обработка ввода для вращения камеры
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

        // Проверка на движение игрока
        if (player.position != lastPlayerPosition)
        {
            isFollowingPlayer = true;
        }

        // Автоматическое возвращение камеры к игроку
        if (timeSinceLastManualRotation >= inactivityReturnDelay && !isFollowingPlayer)
        {
            isFollowingPlayer = true;
        }

        lastPlayerPosition = player.position; // Обновляем последнюю позицию игрока
    }

    void LateUpdate()
    {
        if (isFollowingPlayer)
        {
            Vector3 axisToPlayerDirection = (player.position - centralAxis.position).normalized;
            axisToPlayerDirection.y = 0; // Игнорируем вертикальную компоненту для сохранения горизонтального направления

            Vector3 cameraPosition = centralAxis.position + axisToPlayerDirection * orbitDistance;
            cameraPosition.y = centralAxis.position.y + orbitHeight;

            transform.position = Vector3.Lerp(transform.position, cameraPosition, Time.deltaTime * followSpeed);

            // Устанавливаем угол наклона камеры
            Vector3 relativePosition = centralAxis.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePosition);
            Quaternion tilt = Quaternion.Euler(tiltAngle, rotation.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, tilt, Time.deltaTime * followSpeed);
        }
    }

    void RotateCamera(float direction)
    {
        transform.RotateAround(centralAxis.position, Vector3.up, direction * speedHandleRotation * Time.deltaTime); // Вращение вокруг оси Y
        timeSinceLastManualRotation = 0f;
        isFollowingPlayer = false;
    }
}