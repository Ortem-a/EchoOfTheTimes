using EchoOfTheTimes.UI;
using EchoOfTheTimes.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Training
{
    [RequireComponent(typeof(BoxCollider))]
    public class ToCheckpointUiActivator : MonoBehaviour
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
                EnableCheckpointButton();
            }
        }

        private void EnableCheckpointButton()
        {
            UiManager.Instance.UiSceneController.EnableCheckpointButton();

            gameObject.SetActive(false);
        }
    }
}