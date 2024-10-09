using UnityEngine;

namespace EchoOfTheTimes.Collectables
{
    public class CollectablePlaceholder : MonoBehaviour
    {
        [SerializeField]
        private Color _gizmoColor = Color.magenta;
        [Range(0.1f, 1f)]
        [SerializeField]
        private float _radius = 0.5f;

        private void OnDrawGizmos()
        {
            Gizmos.color = _gizmoColor;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}