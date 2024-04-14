using EchoOfTheTimes.Interfaces;
using EchoOfTheTimes.UI;
using EchoOfTheTimes.Units;
using System;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Core
{
    public class ChapterFinishPoint : MonoBehaviour, ISpecialVertex
    {
        public Action OnEnter => Enter;
        public Action OnExit => null;

        private Player _player;
        private UserInputHandler _inputHandler;

        [Inject]
        public void Construct(Player player, UserInputHandler inputHandler)
        {
            _player = player;
            _inputHandler = inputHandler;
        }

        private void Enter()
        {
#warning ÃÎÂÍÈÙÅÅÅÅ
            //UiManager.Instance.UiSceneController.EnableFinishCanvas();

            _player.Stop(null);

            _inputHandler.gameObject.SetActive(false);
        }
    }
}