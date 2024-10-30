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

        private HUDController _hudController;

        [Inject]
        private void Construct(HUDController hudController)
        {
            _hudController = hudController;
        }

        private void Freeze()
        {
            Debug.Log($"[StateFreezer] Freeze");

            _hudController.DisableButtons();
        }

        private void Cancel()
        {
            Debug.Log($"[StateFreezer] Cancel");

            _hudController.EnableButtons();
            _hudController.EnableButtonPending();
        }
    }
}