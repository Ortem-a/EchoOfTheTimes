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

            _levelAnalyticsTracker.SetStatus();
            _levelAnalyticsTracker.EndLevelAnalytics();

            PersistenceService.OnLevelCompleted?.Invoke(-1);
        }
    }
}
