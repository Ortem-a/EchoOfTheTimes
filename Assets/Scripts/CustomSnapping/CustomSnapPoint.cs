using UnityEngine;

namespace EchoOfTheTimes.CustomSnapping
{
    public class CustomSnapPoint : MonoBehaviour
    {
        [Range(0.01f, 1f)]
        public float Radius = 0.3f;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, Radius);
        }
    }
}