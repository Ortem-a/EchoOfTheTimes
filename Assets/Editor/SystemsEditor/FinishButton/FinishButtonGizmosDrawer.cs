using Systems.Leveling;
using UnityEditor;
using UnityEngine;

namespace SystemsEditor
{
    public static class FinishButtonGizmosDrawer
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Selected | GizmoType.InSelectionHierarchy |
            GizmoType.NotInSelectionHierarchy | GizmoType.NonSelected)]
        private static void DrawGizmos(FinishButton button, GizmoType gizmoType)
        {
            GizmosDrawerHelper.DrawText(button.transform.position, nameof(FinishButton),
                textColor: Color.white, anchor: TextAnchor.MiddleCenter);
        }
    }
}