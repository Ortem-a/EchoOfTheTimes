using Systems.Leveling;
using UnityEditor;

namespace SystemsEditor
{
    public static class StairsGizmosDrawer
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Selected | GizmoType.InSelectionHierarchy |
            GizmoType.NotInSelectionHierarchy | GizmoType.NonSelected)]
        private static void DrawGizmos(Stair stair, GizmoType gizmoType)
        {
#warning онйю врн уг мюдн кх нмн асдер
        }
    }
}