using EchoOfTheTimes.UI;
using UnityEngine;

namespace EchoOfTheTimes.Training
{
    public class TrainingUiDisabler : MonoBehaviour
    {
        private void Start()
        {
            Disable();
        }

        public void Disable()
        {
            UiManager.Instance.UiSceneController.SetActiveBottomPanel(false, 0f);
            UiManager.Instance.UiSceneView.gameObject.SetActive(false);
        }
    }
}