using UnityEngine;

namespace EchoOfTheTimes.Utils
{
    public class BezierCurve : MonoBehaviour
    {
        public Transform P0;
        public Transform P1;
        public Transform P2;
        public Transform P3;

        [Range(0f, 1f)]
        public float T;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            int segmentNumber = 20;
            Vector3 previousPoint = P0.position;

            for (int i = 0; i < segmentNumber; i++)
            {
                float parameter = (float)i / segmentNumber;
                Vector3 point = Bezier.GetPoint(P0.position, P1.position, P2.position, P3.position, parameter);
                Gizmos.DrawLine(previousPoint, point);
                previousPoint = point;
            }
        }
    }
}