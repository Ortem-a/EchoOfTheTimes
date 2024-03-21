using TMPro;
using UnityEngine;

namespace EchoOfTheTimes.UI
{
    public class UiSceneView : MonoBehaviour
    {
        public TMP_Text InfoLabel;

        public void UpdateLabel(int stateId)
        {
            InfoLabel.text = $"State: {stateId}";
        }
    }
}