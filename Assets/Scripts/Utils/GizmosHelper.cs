using EchoOfTheTimes.LevelStates;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Utils
{
    public static class GizmosHelper
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

        public static void DrawWireMeshesByTRS(List<(Mesh mesh, Transform t)> meshes, StateParameter stateParameter)
        {
            foreach (var mesh in meshes)
            {
                DrawWireMeshByTRS(mesh.mesh, mesh.t, stateParameter);
            }
        }

        public static void DrawWireMeshByTRS(Mesh mesh, Transform parent, StateParameter stateParameter)
        {
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(
                stateParameter.Position,
                Quaternion.Euler(stateParameter.Rotation),
                stateParameter.LocalScale);
            Gizmos.matrix = rotationMatrix;

            Gizmos.DrawWireMesh(mesh,
                parent.localPosition,
                parent.localRotation,
                parent.localScale);
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
    }
}