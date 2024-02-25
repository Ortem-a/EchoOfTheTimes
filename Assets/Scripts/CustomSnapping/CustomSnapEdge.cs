using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.CustomSnapping
{
    public class CustomSnapEdge : MonoBehaviour
    {
        [Range(0.001f, 1f)]
        public float Radius = 0.08f;

        public Vector3 Head;
        public Vector3 Tail;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;    
            Gizmos.DrawLine(Head, Tail);
            Gizmos.DrawSphere(transform.position, Radius);
            Gizmos.color = Color.white;    
        }
    }
}