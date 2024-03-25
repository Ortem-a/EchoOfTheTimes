using EchoOfTheTimes.Interfaces;
using EchoOfTheTimes.UI;
using System;
using UnityEngine;

namespace EchoOfTheTimes.Core
{
    public class ChapterFinishPoint : MonoBehaviour, ISpecialVertex
    {
        public Action OnEnter => Enter;
        public Action OnExit => null;

        private void Enter()
        {
            UiManager.Instance.UiSceneController.EnableFinishCanvas();

            GameManager.Instance.UserInputHandler.gameObject.SetActive(false);
        }

        public void Initialize() { }
    }
}