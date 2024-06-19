using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera_Pasha : MonoBehaviour
{
    private Vector3 initialPosition;

    void Start()
    {
        // Сохраняем начальную позицию объекта
        initialPosition = transform.position;
    }

    void Update()
    {
        // Возвращаем объект в его изначальную позицию
        transform.position = initialPosition;

        // Получаем направление от объекта к камере
        Vector3 directionToCamera = Camera.main.transform.position - transform.position;
        directionToCamera.y = 0; // Игнорируем вертикальную компоненту для поворота только по горизонтали

        // Поворачиваем объект к камере
        if (directionToCamera != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
            // Добавляем вращение на 180 градусов
            targetRotation *= Quaternion.Euler(0, 180, 0);
            transform.rotation = targetRotation;
        }
    }
}
