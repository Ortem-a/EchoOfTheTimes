using EchoOfTheTimes.Core;
using EchoOfTheTimes.UI;
using System;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.LevelStates
{
    public class StateFreezer : MonoBehaviour
    {
        public Action OnFreeze => Freeze;
        public Action OnCancel => Cancel;

        private UserInputHandler _inputHandler;
        private UiSceneController _uiSceneController;

        [Inject]
        private void Construct(UserInputHandler userInputHandler, UiSceneController uiSceneController)
        {
            _inputHandler = userInputHandler;
            _uiSceneController = uiSceneController;
        }

        private void Freeze()
        {
            Debug.Log($"[StateFreezer] Freeze");
            _inputHandler.CanChangeStates = false;

            _uiSceneController.SetActiveBottomPanel(false, 0f);
        }

        private void Cancel()
        {
            Debug.Log($"[StateFreezer] Cancel");
            _inputHandler.CanChangeStates = true;

            _uiSceneController.SetActiveBottomPanel(true, 0);
        }
    }
}