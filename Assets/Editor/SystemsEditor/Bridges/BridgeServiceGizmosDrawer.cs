using Systems.Leveling;
using UnityEditor;
using UnityEngine;

namespace SystemsEditor
{
    public static class BridgeServiceGizmosDrawer
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Selected | GizmoType.InSelectionHierarchy |
            GizmoType.NotInSelectionHierarchy | GizmoType.NonSelected)]
        private static void DrawGizmos(BridgeService bridgeService, GizmoType gizmoType)
        {
            Color color = Color.yellow;

            for (int i = 0; i < bridgeService.Bridges.Length; i++)
            {
                if (bridgeService.Bridges[i].IsConnected)
                {
                    color = Color.green;
                }

                Gizmos.color = color;

                if (bridgeService.Bridges[i].Inner == null || bridgeService.Bridges[i].Outer == null) continue;

                Vector3 point1 = bridgeService.Bridges[i].Inner.transform.position;
                Vector3 point2 = bridgeService.Bridges[i].Outer.transform.position;

                GizmosDrawerHelper.DrawBezierCurveBetween(point1, point2, Vector3.up * 1f, 20, color);
            }

            Gizmos.color = Color.white;
        }
    }
}