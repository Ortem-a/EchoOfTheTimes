using Systems.Leveling;
using Systems.Movement;
using UnityEditor;
using UnityEngine;

namespace SystemsEditor
{
    public static class GraphVisibilityGizmosDrawer
    {
        [DrawGizmo(GizmoType.Active | GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
        private static void DrawAllStateablesGizmos(GraphVisibility graph, GizmoType gizmoType)
        {
            Stateable[] stateables = graph.GetComponentsInChildren<Stateable>();

            for (int i = 0; i < stateables.Length; i++)
            {
                if (stateables[i].Options == null || stateables[i].Options.Count == 0) continue;

                foreach (int state in stateables[i].Options.Keys)
                {
                    Gizmos.color = GizmosUtils.GetGizmoColorByState(state);

                    Matrix4x4 matrix = Matrix4x4.TRS(
                        stateables[i].Options[state].LocalPosition,
                        stateables[i].Options[state].LocalRotation,
                        stateables[i].Options[state].LocalScale
                    );

                    GizmosDrawerHelper.DrawHierarchyRecursive(
                        stateables[i].transform,
                        matrix
                    );
                }
            }
        }
    }
}