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
        private void Initialize(UserInputHandler userInputHandler, UiSceneController uiSceneController)
        {
            _inputHandler = userInputHandler;
            _uiSceneController = uiSceneController;
        }

        public void Initialize()
        {
            _inputHandler = GameManager.Instance.UserInputHandler;
        }

        private void Freeze()
        {
            Debug.Log($"[StateFreezer] Freeze");
            _inputHandler.CanChangeStates = false;

            //UiManager.Instance.UiSceneController.SetActiveBottomPanel(false, 0f);
            _uiSceneController.SetActiveBottomPanel(false, 0f);
        }

        private void Cancel()
        {
            Debug.Log($"[StateFreezer] Cancel");
            _inputHandler.CanChangeStates = true;

            //UiManager.Instance.UiSceneController.SetActiveBottomPanel(true, 0);
            _uiSceneController.SetActiveBottomPanel(true, 0);
        }
    }
}