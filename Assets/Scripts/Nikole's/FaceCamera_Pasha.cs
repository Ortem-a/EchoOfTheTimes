using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera_Pasha : MonoBehaviour
{
    private Vector3 initialPosition;

    public float addRotation = 180;
    public bool lockX = false;
    public bool lockY = true;
    public bool lockZ = false;

    // Enum для выбора режима взгляда
    public enum LookMode
    {
        Camera,
        Screen
    }

    public LookMode lookMode = LookMode.Camera;

    void Start()
    {
        // Сохраняем начальную позицию объекта
        initialPosition = transform.position;
    }

    void Update()
    {
        // Возвращаем объект в его изначальную позицию
        transform.position = initialPosition;

        Vector3 directionToTarget;

        // Определяем направление взгляда в зависимости от выбранного режима
        if (lookMode == LookMode.Camera)
        {
            directionToTarget = Camera.main.transform.position - transform.position;
        }
        else
        {
            directionToTarget = Camera.main.transform.forward;
        }

        // Блокировка изменений по осям X, Y, Z если требуется
        if (lockX)
        {
            directionToTarget.x = 0;
        }
        if (lockY)
        {
            directionToTarget.y = 0;
        }
        if (lockZ)
        {
            directionToTarget.z = 0;
        }

        // Поворачиваем объект к цели
        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            // Применяем дополнительное вращение
            targetRotation *= Quaternion.Euler(0, addRotation, 0);
            transform.rotation = targetRotation;
        }
    }
}
