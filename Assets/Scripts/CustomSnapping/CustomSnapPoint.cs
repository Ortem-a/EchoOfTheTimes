using UnityEngine;

namespace EchoOfTheTimes.CustomSnapping
{
    public class CustomSnapPoint : MonoBehaviour
    {
        [Range(0.01f, 1f)]
        public float Radius = 0.3f;

        public Vector3 Position => transform.position;
        public Vector3 LocalPosition => transform.localPosition;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, Radius);
        }

        public override string ToString()
        {
            return $"[{transform.parent.name}]->[{name}] " +
                $"Position: {Position} | Local Position: {LocalPosition} " +
                $"Rotation: {transform.rotation.eulerAngles} | Local Rotation: {transform.localRotation.eulerAngles}";
        }
    }
}