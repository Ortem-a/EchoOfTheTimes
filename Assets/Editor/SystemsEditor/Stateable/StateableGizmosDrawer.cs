using Systems.Leveling;
using UnityEditor;
using UnityEngine;

namespace SystemsEditor
{
    public class StateableGizmosDrawer
    {
        private static Color[] _stateColors = new Color[4]
        {
            Color.red,
            Color.green,
            Color.blue,
            Color.magenta
        };

        [DrawGizmo(GizmoType.Pickable | GizmoType.Selected | GizmoType.InSelectionHierarchy | GizmoType.InSelectionHierarchy)]
        private static void DrawGizmos(Stateable target, GizmoType gizmoType)
        {
            var filters = target.GetComponentsInChildren<MeshFilter>();

            if (filters == null || filters.Length == 0) return;

            if (target.States != null)
            {
                foreach (var item in target.States.Items)
                {
                    Gizmos.color = _stateColors[item.Key];

                    var option = item.Value;

#warning нрдекэмн ярнъыхе нрдекэмн ялнрперэ, ю ху дереи блеяре...бхдхлн...

                    GizmosDrawerHelper.DrawWireMeshesByTRS(filters, target.transform, option);
                }
            }
        }
    }
}