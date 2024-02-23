using UnityEngine;

namespace EchoOfTheTimes.Utils
{
    public class BezierMover : MonoBehaviour
    {
        public Transform P0;
        public Transform P1;
        public Transform P2;
        public Transform P3;

        [Range(0f, 1f)]
        public float T;

        //private float sign = 1f;

        /*
        private void Update()
        {
            transform.position = Bezier.GetPoint(P0.position, P1.position, P2.position, P3.position, T);
            transform.rotation = Quaternion.LookRotation(
                Bezier.GetFirstDerivative(P0.position, P1.position, P2.position, P3.position, T)
                );

            if (T >= 1f)
            {
                sign *= -1;
            }
            if (T < 0f)
            {
                sign *= -1;
            }

            T += sign * Time.deltaTime;
        }
        */

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