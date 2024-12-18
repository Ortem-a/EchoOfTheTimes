using EchoOfTheTimes.Utils;
using Systems.Movement;
using UnityEditor;
using UnityEngine;

public static class VertexGizmosDrawer
{
    [DrawGizmo(GizmoType.Active | GizmoType.Selected | GizmoType.InSelectionHierarchy |
        GizmoType.NotInSelectionHierarchy | GizmoType.NonSelected)]
    private static void DrawGizmos(Vertex vertex, GizmoType gizmoType)
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(vertex.transform.position, 0.15f);

        if (vertex.Neighbours == null) return;

        foreach (var n in vertex.Neighbours)
        {
            if (n.Vertex == null) continue;

            GizmosHelper.DrawArrowBetween(vertex.transform.position, n.Vertex.transform.position, Color.yellow);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(vertex.transform.position, n.Vertex.transform.position);
        }
    }
}
