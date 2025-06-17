using Systems.Leveling;
using UnityEditor;
using UnityEngine;

namespace SystemsEditor
{
    public static class StairsGizmosDrawer
    {
        [DrawGizmo(GizmoType.Active | GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
        private static void DrawGizmos(Stair stair, GizmoType gizmoType)
        {
            if (stair.Stateable == null || stair.Stateable.Options == null) return;

            foreach (int state in stair.Stateable.Options.Keys)
            {
                Gizmos.color = GizmoColorUtils.GetGizmoColorByState(state);

                Matrix4x4 matrix = Matrix4x4.TRS(
                    stair.Stateable.Options[state].LocalPosition,
                    stair.Stateable.Options[state].LocalRotation,
                    stair.Stateable.Options[state].LocalScale
                );

                GizmosDrawerHelper.DrawHierarchyRecursive(
                    stair.Stateable.transform,
                    matrix
                );
            }
        }
    }
}