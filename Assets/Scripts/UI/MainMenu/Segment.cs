using UnityEngine;

namespace EchoOfTheTimes.UI.MainMenu
{
    [RequireComponent(typeof(BoxCollider))]
    public class Segment : MonoBehaviour
    {
        private BoxCollider _collider;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();

            SetEnable(false);
        }

        public void SetEnable(bool isEnable)
        {
            _collider.enabled = isEnable;
        }

        public void HandleTouch()
        {
            Debug.Log($"[{name}] TOUCHED");
        }
    }
}