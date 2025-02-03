using System.Collections.Generic;
using Systems.Leveling;
using UnityEditor;
using UnityEngine;

namespace SystemsEditor
{
    public static class GizmosDrawerHelper
    {
        public static void DrawArrow(Vector3 position, Vector3 direction, Color color,
                float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            Gizmos.color = color;

            Gizmos.DrawRay(position, direction);

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * Vector3.forward;
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * Vector3.forward;
            Gizmos.DrawRay(position + direction, right * arrowHeadLength);
            Gizmos.DrawRay(position + direction, left * arrowHeadLength);
        }

        public static void DrawArrowBetween(Vector3 from, Vector3 to, Color color,
            float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            DrawArrow((from + to) / 2f, (to - from).normalized, color, arrowHeadLength, arrowHeadAngle);
        }

        public static void DrawWireMeshesByTRS(MeshFilter[] meshFilters, Transform parent, StateOption option)
        {
            foreach (var filter in meshFilters)
            {
                DrawWireMeshByTRS(filter.sharedMesh, filter.transform, parent, option);
            }
        }

        public static void DrawWireMeshByTRS(Mesh mesh, Transform t, Transform parent, StateOption option)
        {
            var oldMatrix = Gizmos.matrix;

            Matrix4x4 rotationMatrix = Matrix4x4.TRS(
                //parent.TransformPoint(option.LocalPosition),
                option.LocalPosition,
                option.LocalRotation,
                option.LocalScale);

            Gizmos.matrix = rotationMatrix;

            var pos = rotationMatrix.GetPosition();
            var rot = rotationMatrix.rotation.eulerAngles;
            var scale = rotationMatrix.lossyScale;

            //Gizmos.DrawWireMesh(mesh);

            Gizmos.DrawWireMesh(mesh,
                t.localPosition,
                t.localRotation,
                t.localScale);

            Gizmos.matrix = oldMatrix;
        }

        public static void DrawWireMeshesByTRS(List<(Mesh mesh, Transform t)> meshes,
            Vector3 position, Quaternion rotation, Vector3 localScale)
        {
            foreach (var mesh in meshes)
            {
                DrawWireMeshByTRS(mesh.mesh, mesh.t, position, rotation, localScale);
            }
        }

        public static void DrawWireMeshByTRS(Mesh mesh, Transform parent,
            Vector3 position, Quaternion rotation, Vector3 localScale)
        {
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(position, rotation, localScale);
            Gizmos.matrix = rotationMatrix;

            Gizmos.DrawWireMesh(mesh,
                parent.localPosition,
                parent.localRotation,
                parent.localScale);
        }

        public static void DrawBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int segmentNumber, Color color)
        {
            Gizmos.color = color;
            Vector3 previousPoint = p0;

            for (int i = 0; i <= segmentNumber; i++)
            {
                float parameter = (float)i / segmentNumber;
                Vector3 point = Bezier.GetPoint(p0, p1, p2, p3, parameter);
                Gizmos.DrawLine(previousPoint, point);
                previousPoint = point;
            }
        }

        public static void DrawBezierCurveBetween(Vector3 point1, Vector3 point2, Vector3 curvature, int segmentNumber, Color color)
        {
            Vector3 direction = (point2 - point1).normalized;
            float part = Vector3.Distance(point1, point2) / 3f;

            var p0 = point1;
            var p1 = point1 + part * direction;
            var p2 = point1 + 2f * part * direction;
            var p3 = point1 + 3f * part * direction;

            p1 += curvature;
            p2 += curvature;

            DrawBezierCurve(p0, p1, p2, p3, segmentNumber, color);
        }

        public static void DrawText(Vector3 position, string text, GUIStyle style)
        {
            Handles.Label(position, text, style);
        }

        public static void DrawText(Vector3 position, string text, 
            Color textColor, TextAnchor anchor = TextAnchor.MiddleCenter)
        {
            var style = new GUIStyle
            {
                alignment = anchor
            };
            style.normal.textColor = textColor;

            DrawText(position, text, style);
        }
    }
}