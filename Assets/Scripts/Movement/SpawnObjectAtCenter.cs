using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpawnObjectAtCenter : MonoBehaviour
{
    public GameObject objectToSpawn; // ������ ��� ������

    // ������� ��� ���������� ������ � ������ �������
    public void SpawnAtCenter()
    {
        if (objectToSpawn == null)
        {
            Debug.LogError("Object to spawn is not assigned.");
            return;
        }

        Vector3 center = Vector3.zero;
        int count = transform.childCount;

        if (count == 0)
        {
            Debug.LogError("No child objects found.");
            return;
        }

        // ��������� ����������� ����� ����� �������� ��������
        for (int i = 0; i < count; i++)
        {
            center += transform.GetChild(i).position;
        }
        center /= count;

        // ������ ������ � ����������� �������
        Instantiate(objectToSpawn, center, Quaternion.identity, transform);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SpawnObjectAtCenter))]
public class SpawnObjectAtCenterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // ��������� ������������ ����������

        SpawnObjectAtCenter script = (SpawnObjectAtCenter)target;

        // ���� ������ ������, �������� ������� ������
        if (GUILayout.Button("���������� ������"))
        {
            script.SpawnAtCenter();
        }
    }
}
#endif
