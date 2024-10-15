using UnityEngine;

namespace EchoOfTheTimes.UI.MainMenu
{
    public class FooterCircleEffectsService : MonoBehaviour
    {
        public void MarkAsSelected(bool isSelected, float multiplyer = 1f)
        {
            if (isSelected)
            {
                transform.localScale = Vector3.one * multiplyer;
            }
            else
            {
                transform.localScale = Vector3.one;
            }
        }
    }
}