using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskObject : MonoBehaviour
{
    public GameObject[] ObjMasked;
    private float updateInterval = 0.1f; // Интервал обновления в секундах
    private float timeSinceLastUpdate = 0f;

    void Start()
    {
        foreach (GameObject obj in ObjMasked)
        {
            MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                // Создание экземпляра материала для каждого объекта
                Material instanceMaterial = new Material(meshRenderer.material);
                instanceMaterial.renderQueue = 3002;
                meshRenderer.material = instanceMaterial;
            }
            else
            {
                Debug.LogWarning($"Объект {obj.name} не имеет MeshRenderer.");
            }
        }
    }

    void Update()
    {
        // Проверяем наличие касания
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Проверка на интервал обновления
            timeSinceLastUpdate += Time.deltaTime;
            if (timeSinceLastUpdate >= updateInterval)
            {
                timeSinceLastUpdate = 0f;
                UpdateMaskPosition(touch.position);
            }
        }
    }

    private void UpdateMaskPosition(Vector2 touchPosition)
    {
        Ray castPoint = Camera.main.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(castPoint, out RaycastHit hit, Mathf.Infinity))
        {
            transform.position = hit.point;
        }
    }
}
