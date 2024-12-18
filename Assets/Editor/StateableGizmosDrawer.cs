using EchoOfTheTimes.Utils;
using UnityEditor;
using UnityEngine;

public class StateableGizmosDrawer
{
    [DrawGizmo(GizmoType.Pickable | GizmoType.Selected | GizmoType.InSelectionHierarchy)]
    private static void DrawGizmos(Systems.Leveling.Stateable target, GizmoType gizmoType)
    {
        var mesh = target.GetComponent<MeshFilter>().sharedMesh;

        foreach (var option in target.Options)
        {
            Gizmos.DrawWireMesh(mesh,
                option.Value.LocalPosition,
                option.Value.LocalRotation,
                option.Value.LocalScale
            );
        }
    }
}