using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.UI
{
    public class HUDController : MonoBehaviour
    {
        private List<UiStateButton> _buttons = new List<UiStateButton>();
        private bool _enableButtonsPending;
        private float _enableButtonsTime;
        private bool isNowOkShadow = true;

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
            Debug.Log("ХОРОШИЕ ТЕНИ");
            _enableButtonsPending = true;
            _enableButtonsTime = Time.time + 0.05f; // можно ставить задержку чтобы не залагались анимации
        }

        private void SetShadowsToGood()
        {
            foreach (var button in _buttons)
            {
                button.SetGoodShadowActive(true);
                button.SetBadShadowActive(false);
            }
        }

        private void SetShadowsToBad()
        {
            Debug.Log("ПЛОХИЕ ТЕНИ");
            foreach (var button in _buttons)
            {
 
                button.SetGoodShadowActive(false);
                button.SetBadShadowActive(true);
                
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
