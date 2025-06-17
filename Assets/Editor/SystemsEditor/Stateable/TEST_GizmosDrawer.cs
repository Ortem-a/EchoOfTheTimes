using UnityEngine;
using System.Collections.Generic;
using Systems;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class StateOptionGizmosDrawer
{
#if UNITY_EDITOR
    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    private static void DrawStateableGizmos(Stateable stateable, GizmoType gizmoType)
    {
        if (stateable == null || stateable.stateOptions == null) return;

        foreach (StateOption option in stateable.stateOptions)
        {
            if (string.IsNullOrEmpty(option.id)) continue;
            
            // Устанавливаем цвет на основе ID
            Gizmos.color = GetColorFromID(option.id);
            
            // Рекурсивная отрисовка иерархии
            DrawHierarchyRecursive(
                stateable.transform, 
                option.position,
                option.rotation,
                option.scale
            );
        }
    }

    private static void DrawHierarchyRecursive(
        Transform current,
        Vector3 rootPosition,
        Quaternion rootRotation,
        Vector3 rootScale
    ) {
        // Вычисляем мировую матрицу для текущего объекта
        Matrix4x4 matrix = Matrix4x4.TRS(rootPosition, rootRotation, rootScale) 
                         * current.localToWorldMatrix;

        // Отрисовываем меш если он есть
        MeshFilter meshFilter = current.GetComponent<MeshFilter>();
        if (meshFilter != null && meshFilter.sharedMesh != null)
        {
            Gizmos.DrawMesh(
                meshFilter.sharedMesh,
                matrix.GetPosition(),
                matrix.rotation,
                matrix.lossyScale
            );
        }

        // Рекурсивно обрабатываем потомков
        foreach (Transform child in current)
        {
            DrawHierarchyRecursive(
                child,
                rootPosition,
                rootRotation,
                rootScale
            );
        }
    }

    private static Color GetColorFromID(string id)
    {
        // Генерация цвета на основе хеша ID
        int hash = id.GetHashCode();
        return new Color(
            (hash & 0xFF) / 255f,
            ((hash >> 8) & 0xFF) / 255f,
            ((hash >> 16) & 0xFF) / 255f,
            0.5f // Полупрозрачность
        );
    }
#endif
}