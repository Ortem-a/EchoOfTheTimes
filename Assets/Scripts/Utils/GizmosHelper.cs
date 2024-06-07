using EchoOfTheTimes.LevelStates;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Utils
{
    public static class GizmosHelper
    {
        public static void DrawArrow(Vector3 position, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            Gizmos.color = color;

            Gizmos.DrawRay(position, direction);

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(position + direction, right * arrowHeadLength);
            Gizmos.DrawRay(position + direction, left * arrowHeadLength);

            Gizmos.color = Color.white;
        }

        public static void DrawArrowBetween(Vector3 from, Vector3 to, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            DrawArrow((from + to) / 2f, (to - from).normalized, color, arrowHeadLength, arrowHeadAngle);
        }

        //public static void DrawWireMeshesByTRS(List<(Mesh mesh, Transform t)> meshes, StateParameter stateParameter)
        public static void DrawWireMeshesByTRS(List<(Mesh mesh, Transform t)> meshes, IStateParameter stateParameter)
        {
            foreach (var mesh in meshes)
            {
                DrawWireMeshByTRS(mesh.mesh, mesh.t, stateParameter);
            }
        }

        //public static void DrawWireMeshByTRS(Mesh mesh, Transform parent, StateParameter stateParameter)
        public static void DrawWireMeshByTRS(Mesh mesh, Transform parent, IStateParameter stateParameter)
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
    }
}