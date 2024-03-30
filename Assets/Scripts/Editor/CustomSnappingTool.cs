using EchoOfTheTimes.CustomSnapping;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.EditorTools;
#endif
using UnityEngine;

namespace EchoOfTheTimes.Editor
{
#if UNITY_EDITOR
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
            CustomSnapEdge[] allEdges = FindObjectsOfType<CustomSnapEdge>();
            CustomSnapEdge[] targetEdges = targetTransform.GetComponentsInChildren<CustomSnapEdge>();

            Vector3 bestPosition = newPosition;
            float closestDistance = float.PositiveInfinity;

            foreach (CustomSnapEdge edge in allEdges)
            {
                if (edge.transform.parent == targetTransform) continue;

                foreach (CustomSnapEdge ownEdge in targetEdges)
                {
                    Vector3 targetPos = edge.transform.position - (ownEdge.transform.position - targetTransform.position);
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
#endif
}