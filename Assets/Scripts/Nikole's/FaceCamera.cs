using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public float addRotation = 0;

    void Update()
    {
        Vector3 cameraDir = Camera.main.transform.forward;
        // Опционально: обнуляем изменение по оси Y, если объект должен вращаться только в горизонтальной плоскости
        // cameraDir.y = 0; 

        // Создаем кватернион ориентации, добавляя 90 градусов по оси Y
        Quaternion rotation = Quaternion.LookRotation(cameraDir);
        Quaternion additionalRotation = Quaternion.Euler(0, addRotation, 0); // добавляем 90 градусов вращения вокруг оси Y

        // Применяем итоговое вращение к объекту
        transform.rotation = rotation * additionalRotation;
    }
}
