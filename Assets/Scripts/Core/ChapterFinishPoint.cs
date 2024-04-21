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
        private InputMediator _inputHandler;
        private UiSceneController _sceneController;

        [Inject]
        public void Construct(Player player, InputMediator inputHandler, UiSceneController sceneController)
        {
            _player = player;
            _inputHandler = inputHandler;
            _sceneController = sceneController;
        }

        private void Enter()
        {
            _sceneController.EnableFinishCanvas();

            _player.Stop(null);

            _inputHandler.gameObject.SetActive(false);
        }
    }
}