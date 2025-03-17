using Systems.Movement;
using Systems.Units;
using UnityEditor;
using UnityEngine;

namespace SystemsEditor
{
    public static class IMovableByPathGizmosDrawer
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Selected | GizmoType.InSelectionHierarchy |
            GizmoType.NotInSelectionHierarchy | GizmoType.NonSelected)]
        private static void DrawGizmos(IMovableByPath movable, GizmoType gizmoType)
        {
            if (movable.Direction != default)
            {
                GizmosDrawerHelper.DrawArrow(movable.CurrentWaypoint.transform.position, movable.Direction, Color.magenta);
            }

            if (movable.Path != null)
            {
                Gizmos.color = Color.blue;

                foreach (Vertex v in movable.Path)
                {
                    Gizmos.DrawSphere(v.transform.position, 0.15f);
                }
            }
        }
    }
}