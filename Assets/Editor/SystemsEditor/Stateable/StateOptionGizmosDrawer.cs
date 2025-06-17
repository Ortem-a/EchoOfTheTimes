using Systems.Leveling;
using UnityEditor;
using UnityEngine;

namespace SystemsEditor
{
    public static class StateOptionGizmosDrawer
    {
        [DrawGizmo(GizmoType.Active | GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
        private static void DrawStateableGizmos(Stateable stateable, GizmoType gizmoType)
        {
            if (stateable == null || stateable.Options == null) return;

            foreach (int state in stateable.Options.Keys)
            {
                Gizmos.color = GizmoColorUtils.GetGizmoColorByState(state);

                Matrix4x4 matrix = Matrix4x4.TRS(
                    stateable.Options[state].LocalPosition,
                    stateable.Options[state].LocalRotation,
                    stateable.Options[state].LocalScale
                );

                GizmosDrawerHelper.DrawHierarchyRecursive(
                    stateable.transform,
                    matrix
                );
            }
        }
    }
}