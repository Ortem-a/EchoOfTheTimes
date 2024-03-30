using EchoOfTheTimes.Core;
using EchoOfTheTimes.UI;
using System;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    public class StateFreezer : MonoBehaviour
    {
        public Action OnFreeze => Freeze;
        public Action OnCancel => Cancel;

        private UserInputHandler _inputHandler;

        public void Initialize()
        {
            _inputHandler = GameManager.Instance.UserInputHandler;
        }

        private void Freeze()
        {
            Debug.Log($"[StateFreezer] Freeze");
            _inputHandler.CanChangeStates = false;

            UiManager.Instance.UiSceneController.SetActiveBottomPanel(false, 0f);
        }

        private void Cancel()
        {
            Debug.Log($"[StateFreezer] Cancel");
            _inputHandler.CanChangeStates = true;

            UiManager.Instance.UiSceneController.SetActiveBottomPanel(true, 0);
        }
    }
}