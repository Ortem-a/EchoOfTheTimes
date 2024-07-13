using EchoOfTheTimes.UI;
using System;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Core
{
    public class VertexTrigger : MonoBehaviour
    {
        public Action OnPlayerEnter => PlayerEnter;

        private UiSceneController _uiSceneController;

        [Inject]
        private void Construct(UiSceneController uiSceneController)
        {
            _uiSceneController = uiSceneController;
        }

        private void PlayerEnter()
        {
            Debug.Log($"[VertexTrigger] Player Enter");
            //_uiSceneController.DisableBottomPanelPermanently();
        }
    }
}
