using Systems.Movement;
using UnityEditor;
using UnityEngine;

namespace SystemsEditor
{
    public static class VertexGizmosDrawer
    {
        [DrawGizmo(GizmoType.Active | GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
        private static void DrawGizmos(Vertex vertex, GizmoType gizmoType)
        {
            if (vertex.IsMoving)
            {
                Gizmos.color = GizmoColorUtils.MovingColor();
            }
            else
            {
                Gizmos.color = GizmoColorUtils.StaticColor();
            }

            Gizmos.DrawSphere(vertex.transform.position, 0.15f);

            if (vertex.Neighbours == null) return;

            foreach (var n in vertex.Neighbours)
            {
                if (n.Vertex == null) continue;

                GizmosDrawerHelper.DrawArrowBetween(vertex.transform.position, n.Vertex.transform.position, GizmoColorUtils.ArrowColor());

                if (vertex.IsBridge && n.Vertex.IsBridge)
                {
                    Gizmos.color = GizmoColorUtils.BridgeColor();
                }
                else
                {
                    Gizmos.color = GizmoColorUtils.ConnectionColor();
                }

                Gizmos.DrawLine(vertex.transform.position, n.Vertex.transform.position);
            }

            GizmosDrawerHelper.DrawText(
                vertex.transform.position + Vector3.up * 0.4f,
                vertex.Id.ToString(),
                textColor: GizmoColorUtils.TextColor(),
                anchor: TextAnchor.MiddleCenter);
        }
    }
}