using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.UI
{
    public class HUDController : MonoBehaviour
    {
        private List<UiStateButton> _buttons = new List<UiStateButton>();
        private bool _enableButtonsPending;
        private float _enableButtonsTime;

        public void RegisterButton(UiStateButton button)
        {
            _buttons.Add(button);
        }

        public void DisableButtons()
        {
            _enableButtonsPending = false;
            foreach (var button in _buttons)
            {
                button.SetInteractable(false);
            }
        }

        public void EnableButtons()
        {
            _enableButtonsPending = true;
            _enableButtonsTime = Time.time + 0.05f; // можно ставить задержку чтобы не залагались анимации
        }

        public void EnableButtonsImmediately()
        {
            _enableButtonsPending = false;
            foreach (var button in _buttons)
            {
                button.SetInteractable(true);
            }
        }

        private void Update()
        {
            if (_enableButtonsPending && Time.time >= _enableButtonsTime)
            {
                _enableButtonsPending = false;
                foreach (var button in _buttons)
                {
                    button.SetInteractable(true);
                }
            }
        }
    }
}
