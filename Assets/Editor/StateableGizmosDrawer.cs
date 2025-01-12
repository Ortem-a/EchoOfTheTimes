using UnityEditor;
using UnityEngine;

public class StateableGizmosDrawer
{
    [DrawGizmo(GizmoType.Pickable | GizmoType.Selected | GizmoType.InSelectionHierarchy)]
    private static void DrawGizmos(Systems.Leveling.Stateable target, GizmoType gizmoType)
    {
        var meshFilter = target.GetComponent<MeshFilter>();
        if (meshFilter == null) return;

        var mesh = meshFilter.sharedMesh;

        //foreach (var option in target.Options)
        //{
        //Gizmos.DrawWireMesh(mesh,
        //    option.LocalPosition,
        //    option.LocalRotation,
        //    option.LocalScale
        //);
        //}

        if (target.States != null)
        {
            foreach (var option in target.States.Items)
            {
                Gizmos.DrawWireMesh(mesh,
                    option.Value.LocalPosition,
                    option.Value.LocalRotation,
                    option.Value.LocalScale
                    );
            }
        }
    }
}