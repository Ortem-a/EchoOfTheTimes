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
        private LevelAnalyticsTracker _levelAnalyticsTracker;

        // Убираем ссылку на CollectableService
        [Inject]
        public void Construct(Player player, InputMediator inputHandler, UiSceneController sceneController)
        {
            _player = player;
            _inputHandler = inputHandler;
            _sceneController = sceneController;
        }

        private void Start()
        {
            _levelAnalyticsTracker = FindObjectOfType<LevelAnalyticsTracker>();
        }

        private void Enter()
        {
            _sceneController.EnableFinishCanvas();
            _player.Stop(null);

            // Здесь можно либо удалить обновление аналитики коллектаблов,
            // либо передать фиксированные значения (например, 0)
            int collected = 0;
            int maxCollectables = 0;
            _levelAnalyticsTracker.SetStatus(collected, maxCollectables);
            _levelAnalyticsTracker.EndLevelAnalytics();

            // Если событие OnLevelCompleted больше не нужно, его можно убрать,
            // либо вызывать с фиксированным значением.
            PersistenceService.OnLevelCompleted?.Invoke(collected);
        }
    }
}
