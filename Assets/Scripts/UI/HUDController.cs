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

        // ��������������� ����� EnableButtons
        public void EnableButtons()
        {
            // ��������� ��������� ������ ����� 0.05 ������
            ScheduleEnableButtons(0.05f);
        }

        // ��������������� ����� EnableButtonPending
        public void EnableButtonPending()
        {
            // ��������� ����� ���� �� ������� ����������
            ScheduleShadowsToGood(0f);
        }

        // ����� ��� ������������ ����� ���� �� ������� ����� �������� ��������
        public void ScheduleShadowsToGood(float delay)
        {
            _changeShadowsPending = true;
            _changeShadowsTime = Time.time + delay;
        }

        // ����� ��� ������������ ��������� ������ ����� �������� ��������
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
            // ���������, ���� �� ������� ���� �� �������
            if (_changeShadowsPending && Time.time >= _changeShadowsTime)
            {
                _changeShadowsPending = false;
                SetShadowsToGood();
            }

            // ���������, ���� �� ������������ ������
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
