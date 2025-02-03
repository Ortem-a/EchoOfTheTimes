using Systems.Leveling;
using UnityEditor;
using UnityEngine;

namespace SystemsEditor
{
    public static class LevelButtonGizmosDrawer
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Selected | GizmoType.InSelectionHierarchy |
            GizmoType.NotInSelectionHierarchy | GizmoType.NonSelected)]
        private static void DrawGizmos(LevelButton button, GizmoType gizmoType)
        {
            if (button.Stateables != null && button.Stateables.Length != 0)
            {
                Color color = Color.cyan;

                for (int i = 0; i < button.Stateables.Length; i++)
                {
                    Vector3 point1 = button.transform.position;
                    Vector3 point2 = button.Stateables[i].transform.position;

                    GizmosDrawerHelper.DrawBezierCurveBetween(point1, point2, Vector3.up * 4f, 20, color);
                    Gizmos.DrawSphere(point2, 0.5f);
                }
            }

            GizmosDrawerHelper.DrawText(button.transform.position, nameof(LevelButton),
                textColor: Color.white, anchor: TextAnchor.MiddleCenter);
        }
    }
}