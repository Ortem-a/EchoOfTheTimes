using EchoOfTheTimes.CustomSnapping;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace EchoOfTheTimes.EditorTools
{
    [EditorTool("Level Builder", typeof(CustomSnap))]
    public class CustomSnappingTool : EditorTool
    {
        public Texture2D ToolIcon;

        public float DistanceToSnap = 0.5f;

        public override GUIContent toolbarIcon
        {
            get
            {
                return new GUIContent
                {
                    image = ToolIcon,
                    text = "LB",
                    tooltip = "Level Builder Tool"
                };
            }
        }

        public override void OnToolGUI(EditorWindow window)
        {
            Transform targetTransform = ((CustomSnap)target).transform;

            EditorGUI.BeginChangeCheck();
            Vector3 newPosition = Handles.PositionHandle(targetTransform.position, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(targetTransform, "Move with builder tool");
                MoveWithSnapping(targetTransform, newPosition);
            }
        }

        private void MoveWithSnapping(Transform targetTransform, Vector3 newPosition)
        {
            CustomSnapPoint[] allPoints = FindObjectsOfType<CustomSnapPoint>();
            CustomSnapPoint[] targetPoints = targetTransform.GetComponentsInChildren<CustomSnapPoint>();

            Vector3 bestPosition = newPosition;
            float closestDistance = float.PositiveInfinity;

            foreach (CustomSnapPoint point in allPoints)
            {
                if (point.transform.parent == targetTransform) continue;

                foreach (CustomSnapPoint ownPoint in targetPoints)
                {
                    Vector3 targetPos = point.transform.position - (ownPoint.transform.position - targetTransform.position);
                    float distance = Vector3.Distance(targetPos, newPosition);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        bestPosition = targetPos;
                    }
                }
            }

            if (closestDistance < DistanceToSnap)
            {
                targetTransform.position = bestPosition;
            }
            else
            {
                targetTransform.position = newPosition;
            }
        }
    }
}