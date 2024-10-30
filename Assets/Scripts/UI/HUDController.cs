using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.UI
{
    public class HUDController : MonoBehaviour
    {
        private readonly List<UiStateButton> _buttons = new List<UiStateButton>();

        private bool _enableButtonsPending;
        private float _enableButtonsTime;

        private bool _changeShadowsPending;
        private float _changeShadowsTime;

        public void RegisterButton(UiStateButton button)
        {
            _buttons.Add(button);
        }

        public void DisableButtons()
        {
            _enableButtonsPending = false;
            _changeShadowsPending = false;

            foreach (var button in _buttons)
            {
                button.ChangeInteractable(false);
            }

            SetShadowsToBad();
        }

        // Восстанавливаем метод EnableButtons
        public void EnableButtons()
        {
            // Планируем активацию кнопок через 0.05 секунд
            ScheduleEnableButtons(0.05f);
        }

        // Восстанавливаем метод EnableButtonPending
        public void EnableButtonPending()
        {
            // Планируем смену тени на хорошую немедленно
            ScheduleShadowsToGood(0f);
        }

        // Метод для планирования смены тени на хорошую через заданную задержку
        public void ScheduleShadowsToGood(float delay)
        {
            _changeShadowsPending = true;
            _changeShadowsTime = Time.time + delay;
        }

        // Метод для планирования активации кнопок через заданную задержку
        public void ScheduleEnableButtons(float delay)
        {
            _enableButtonsPending = true;
            _enableButtonsTime = Time.time + delay;
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
            _changeShadowsPending = false;

            foreach (var button in _buttons)
            {
                button.ChangeInteractable(true);
                button.ChangeShadowToGood();
            }
        }

        private void Update()
        {
            // Проверяем, пора ли сменить тень на хорошую
            if (_changeShadowsPending && Time.time >= _changeShadowsTime)
            {
                _changeShadowsPending = false;
                SetShadowsToGood();
            }

            // Проверяем, пора ли активировать кнопки
            if (_enableButtonsPending && Time.time >= _enableButtonsTime)
            {
                _enableButtonsPending = false;

                foreach (var button in _buttons)
                {
                    button.ChangeInteractable(true);
                }
            }
        }
    }
}
