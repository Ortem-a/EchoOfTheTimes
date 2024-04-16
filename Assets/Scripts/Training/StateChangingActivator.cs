using EchoOfTheTimes.Core;
using EchoOfTheTimes.Units;
using UnityEngine;

namespace EchoOfTheTimes.Training
{
    [RequireComponent(typeof(BoxCollider))]
    public class StateChangingActivator : MonoBehaviour
    {
        public Artifact Artifact;

        private BoxCollider _collider;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _collider.isTrigger = true;

            Artifact.gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                EnableStatesChanging();
            }
        }

        private void EnableStatesChanging()
        {
            Artifact.gameObject.SetActive(true);
            Artifact.Enable();

            //UiManager.Instance.UiSceneController.SetActiveBottomPanel(true);
            //UiManager.Instance.UiSceneView.InfoLabel.gameObject.SetActive(true);

            gameObject.SetActive(false);
        }
    }
}