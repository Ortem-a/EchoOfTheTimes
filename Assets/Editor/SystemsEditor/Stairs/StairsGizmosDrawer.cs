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
            if (stair == null || stair.Options == null) return;

            foreach (int state in stair.Options.Keys)
            {
                Gizmos.color = GizmoColorUtils.GetGizmoColorByState(state);

                Matrix4x4 matrix = Matrix4x4.TRS(
                    stair.Options[state].LocalPosition,
                    stair.Options[state].LocalRotation,
                    stair.Options[state].LocalScale
                );

                GizmosDrawerHelper.DrawHierarchyRecursive(
                    stair.transform,
                    matrix
                );
            }
        }
    }
}