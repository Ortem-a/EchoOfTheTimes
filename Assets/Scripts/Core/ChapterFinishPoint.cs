using EchoOfTheTimes.Collectables;
using EchoOfTheTimes.Interfaces;
using EchoOfTheTimes.Persistence;
using EchoOfTheTimes.SceneManagement;
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
        private CollectableService _collectableService;
        private LevelAnalyticsTracker _levelAnalyticsTracker; // Добавлено поле для трекера аналитики

        [Inject]
        public void Construct(Player player, InputMediator inputHandler, UiSceneController sceneController,
            CollectableService collectableService)
        {
            _player = player;
            _inputHandler = inputHandler;
            _sceneController = sceneController;
            _collectableService = collectableService;
        }

        private void Start()
        {
            _levelAnalyticsTracker = FindObjectOfType<LevelAnalyticsTracker>();
        }

        private void Enter()
        {
            _sceneController.EnableFinishCanvas();

            _player.Stop(null);

            // Обновляем данные аналитики перед отправкой
            int collected = _collectableService.CollectedResult;
            int maxCollectables = -1; // _collectableService.MaxCollectablesInLevel;

            _levelAnalyticsTracker.UpdateCollectables(collected);
            _levelAnalyticsTracker.SetStatus(collected, maxCollectables);

            _levelAnalyticsTracker.EndLevelAnalytics();

            PersistenceService.OnLevelCompleted?.Invoke(_collectableService.CollectedResult);
        }
    }
}
