using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.CustomSnapping
{
    public class CustomSnapEdge : MonoBehaviour
    {
        [Range(0.001f, 1f)]
        public float Radius = 0.08f;

        public CustomSnapPoint Head;
        public CustomSnapPoint Tail;

        public Vector3 Rotation => transform.rotation.eulerAngles;
        public Vector3 LocalRotation => transform.localRotation.eulerAngles;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;

            if (Head != null && Tail != null)
            {
                Gizmos.DrawLine(Head.Position, Tail.Position);
            }
            Gizmos.DrawSphere(transform.position, Radius);

            Gizmos.color = Color.white;    
        }

        public override string ToString()
        {
            return $"[{transform.parent.name}]->[{name}] Head: <{Head.name}> | Tail: <{Tail.name}> | " +
                $"Position: {transform.position} | Local Position: {transform.localPosition} " +
                $"Rotation: {Rotation} | Local Rotation: {LocalRotation}";
        }
    }
}