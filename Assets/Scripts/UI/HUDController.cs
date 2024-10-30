using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.UI
{
    public class HUDController : MonoBehaviour
    {
        private readonly List<UiStateButton> _buttons = new List<UiStateButton>();
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
                button.ChangeInteractable(false);
            }

            SetShadowsToBad();
        }

        public void EnableButtons()
        {
            // _enableButtonsPending = true; перенёс в EnableButtonPending тк запускаем посреди смены состояний
            _enableButtonsTime = Time.time + 0.05f; // можно ставить задержку чтобы не залагались анимации
        }

        public void EnableButtonPending()
        {
            _enableButtonsPending = true;
        }

        private void SetShadowsToGood()
        {
            foreach (var button in _buttons)
            {
                button.ChangeShadowToGood();
            }
        }

        private void SetShadowsToBad()
        {
            foreach (var button in _buttons)
            {
                button.ChangeShadowToBad();
            }
        }

        public void EnableButtonsImmediately()
        {
            _enableButtonsPending = false;
            foreach (var button in _buttons)
            {
                button.ChangeInteractable(true);
            }
        }

        private void Update()
        {
            if (_enableButtonsPending && Time.time >= _enableButtonsTime)
            {
                _enableButtonsPending = false;
                foreach (var button in _buttons)
                {
                    button.ChangeInteractable(true);

                    SetShadowsToGood();
                }
            }
        }
    }
}
