using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.UI
{
    public class HUDController : MonoBehaviour
    {
        private List<UiStateButton> _buttons = new List<UiStateButton>();

        public void RegisterButton(UiStateButton button)
        {
            _buttons.Add(button);
        }

        public void DisableButtons()
        {
            foreach (var button in _buttons)
            {
                button.SetInteractable(false);
            }
        }

        public void EnableButtons()
        {
            foreach (var button in _buttons)
            {
                button.SetInteractable(true);
            }
        }
    }
}

