using Systems.Leveling;
using UnityEditor;
using UnityEngine;

namespace SystemsEditor
{
    public static class StateableGizmosDrawer
    {
        [DrawGizmo(GizmoType.Active | GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
        private static void DrawStateableGizmos(Stateable stateable, GizmoType gizmoType)
        {
            if (stateable == null || stateable.Options == null)
                return;

            foreach (int state in stateable.Options.Keys)
            {
                if (stateable.Options[state] == null || stateable.Options[state].Target == null)
                    continue;

                // ÷вет по ID
                Gizmos.color = GizmoColorUtils.GetGizmoColorByState(state);

                // “рансформаци€ только по данным из StateOption
                Matrix4x4 rootMatrix = Matrix4x4.TRS(
                    stateable.Options[state].LocalPosition,
                    stateable.Options[state].LocalRotation,
                    stateable.Options[state].LocalScale
                    );

                // –екурсивный вызов Ч без использовани€ Transform в сцене
                GizmosDrawerHelper.DrawHierarchyRecursive(stateable.transform, rootMatrix);
            }
        }
    }
}