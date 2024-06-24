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

    // Enum ��� ������ ������ �������
    public enum LookMode
    {
        Camera,
        Screen
    }

    public LookMode lookMode = LookMode.Camera;

    void Start()
    {
        // ��������� ��������� ������� �������
        initialPosition = transform.position;
    }

    void Update()
    {
        // ���������� ������ � ��� ����������� �������
        transform.position = initialPosition;

        Vector3 directionToTarget;

        // ���������� ����������� ������� � ����������� �� ���������� ������
        if (lookMode == LookMode.Camera)
        {
            directionToTarget = Camera.main.transform.position - transform.position;
        }
        else
        {
            directionToTarget = Camera.main.transform.forward;
        }

        // ���������� ��������� �� ���� X, Y, Z ���� ���������
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

        // ������������ ������ � ����
        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            // ��������� �������������� ��������
            targetRotation *= Quaternion.Euler(0, addRotation, 0);
            transform.rotation = targetRotation;
        }
    }
}
