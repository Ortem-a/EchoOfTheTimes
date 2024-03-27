using EchoOfTheTimes.UI;
using EchoOfTheTimes.Units;
using UnityEngine;

namespace EchoOfTheTimes.Training
{
    [RequireComponent(typeof(BoxCollider))]
    public class StateChangingActivator : MonoBehaviour
    {
        private BoxCollider _collider;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _collider.isTrigger = true;
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
            UiManager.Instance.UiSceneController.SetActiveBottomPanel(true);
            UiManager.Instance.UiSceneView.InfoLabel.gameObject.SetActive(true);

            gameObject.SetActive(false);
        }
    }
}