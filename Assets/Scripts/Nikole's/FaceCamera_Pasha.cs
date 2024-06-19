using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera_Pasha : MonoBehaviour
{
    private Vector3 initialPosition;

    void Start()
    {
        // ��������� ��������� ������� �������
        initialPosition = transform.position;
    }

    void Update()
    {
        // ���������� ������ � ��� ����������� �������
        transform.position = initialPosition;

        // �������� ����������� �� ������� � ������
        Vector3 directionToCamera = Camera.main.transform.position - transform.position;
        directionToCamera.y = 0; // ���������� ������������ ���������� ��� �������� ������ �� �����������

        // ������������ ������ � ������
        if (directionToCamera != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
            // ��������� �������� �� 180 ��������
            targetRotation *= Quaternion.Euler(0, 180, 0);
            transform.rotation = targetRotation;
        }
    }
}
