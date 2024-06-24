using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public float addRotation = 0;

    void Update()
    {
        Vector3 cameraDir = Camera.main.transform.forward;
        // �����������: �������� ��������� �� ��� Y, ���� ������ ������ ��������� ������ � �������������� ���������
        // cameraDir.y = 0; 

        // ������� ���������� ����������, �������� 90 �������� �� ��� Y
        Quaternion rotation = Quaternion.LookRotation(cameraDir);
        Quaternion additionalRotation = Quaternion.Euler(0, addRotation, 0); // ��������� 90 �������� �������� ������ ��� Y

        // ��������� �������� �������� � �������
        transform.rotation = rotation * additionalRotation;
    }
}
