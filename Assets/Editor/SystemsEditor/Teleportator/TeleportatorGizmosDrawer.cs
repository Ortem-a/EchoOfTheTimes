using Systems.Leveling;
using UnityEditor;
using UnityEngine;

namespace SystemsEditor
{
    public static class TeleportatorGizmosDrawer
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Selected | GizmoType.InSelectionHierarchy |
            GizmoType.NotInSelectionHierarchy | GizmoType.NonSelected)]
        private static void DrawGizmos(Teleportator teleportator, GizmoType gizmoType)
        {
            if (teleportator.Destination != null)
            {
                Color color = Color.blue;

                Vector3 point1 = teleportator.transform.position;
                Vector3 point2 = teleportator.Destination.transform.position;

                GizmosDrawerHelper.DrawBezierCurveBetween(point1, point2, Vector3.up * 2f, 20, color);
            }

            GizmosDrawerHelper.DrawText(teleportator.transform.position, nameof(Teleportator),
                textColor: Color.white, anchor: TextAnchor.MiddleCenter);
        }
    }
}