using EchoOfTheTimes.Collectables;
using EchoOfTheTimes.Interfaces;
using EchoOfTheTimes.Persistence;
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
        // private InputMediator _inputHandler;
        private UiSceneController _sceneController;
        private CollectableService _collectableService;

        [Inject]
        public void Construct(Player player, InputMediator inputHandler, UiSceneController sceneController, 
            CollectableService collectableService)
        {
            _player = player;
            // _inputHandler = inputHandler;
            _sceneController = sceneController;
            _collectableService = collectableService;
        }

        private void Enter()
        {
            _sceneController.EnableFinishCanvas();

            _player.Stop(null);

            // _inputHandler.gameObject.SetActive(false);

            PersistenceService.OnLevelCompleted?.Invoke(_collectableService.CollectedResult);
        }
    }
}