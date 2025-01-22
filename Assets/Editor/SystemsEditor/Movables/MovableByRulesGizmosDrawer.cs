using Systems;
using UnityEditor;

namespace SystemsEditor
{
    public class MovableByRulesGizmosDrawer
    {
        [DrawGizmo(GizmoType.Pickable | GizmoType.Selected | GizmoType.InSelectionHierarchy)]
        private static void DrawGizmos(TestMovableByRules target, GizmoType gizmoType)
        {
        }
    }
}