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

        private UiSceneController _uiSceneController;

        [Inject]
        private void Construct(UiSceneController uiSceneController)
        {
            _uiSceneController = uiSceneController;
        }

        private void Freeze()
        {
            Debug.Log($"[StateFreezer] Freeze");

            _uiSceneController.SetActiveBottomPanel(false, 0f);
        }

        private void Cancel()
        {
            Debug.Log($"[StateFreezer] Cancel");

            _uiSceneController.SetActiveBottomPanel(true, 0f);
        }
    }
}