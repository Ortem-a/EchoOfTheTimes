using Systems;
using UnityEditor;

public class MovableByRulesGizmosDrawer
{
    [DrawGizmo(GizmoType.Pickable | GizmoType.Selected | GizmoType.InSelectionHierarchy)]
    private static void DrawGizmos(TestMovableByRules target, GizmoType gizmoType)
    {
    }
}
