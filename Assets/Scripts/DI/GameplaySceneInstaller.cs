using EchoOfTheTimes.Core;
using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Movement;
using EchoOfTheTimes.ScriptableObjects;
using EchoOfTheTimes.UI;
using EchoOfTheTimes.Units;
using System;
using UnityEditorInternal;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.DI
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        [Header("Systems")]
        [SerializeField]
        private LevelStateMachine _stateMachine;
        [SerializeField]
        private GraphVisibility _graph;
        [SerializeField]
        private CheckpointManager _checkpointManager;
        [SerializeField]
        private VerticesBlocker _verticesBlocker;

        [Header("Player")]
        [SerializeField]
        private RefinedOrbitCamera _camera;
        [SerializeField]
        private Player _player;
        [SerializeField]
        private VertexFollower _vertexFollower;
        [SerializeField]
        private UserInput _userInput;
        [SerializeField]
        private UserInputHandler _userInputHandler;

        [Header("Scriptable Objects")]
        [SerializeField]
        private ColorStateSettingsScriptableObject _colorStateSettings;
        [SerializeField]
        private PlayerSettingsScriptableObject _playerSettings;
        [SerializeField]
        private LevelSettingsScriptableObject _levelSettings;

        [Header("UI")]
        [SerializeField]
        private UiSceneController _uiSceneController;
        [SerializeField]
        private UiSceneView _uiSceneView;

        public override void InstallBindings()
        {
            BindScriptableObjects();
            BindSystems();
            BindPlayer();
            BindUi();

            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void BindSystems()
        {
            Container.Bind<LevelStateMachine>().FromInstance(_stateMachine).AsSingle();
            Container.Bind<GraphVisibility>().FromInstance(_graph).AsSingle();
            Container.Bind<CheckpointManager>().FromInstance(_checkpointManager).AsSingle();
            Container.Bind<VerticesBlocker>().FromInstance(_verticesBlocker).AsSingle();
        }

        private void BindPlayer()
        {
            Container.Bind<VertexFollower>().FromInstance(_vertexFollower).AsSingle();
            Container.Bind<Player>().FromInstance(_player).AsSingle();
            Container.Bind<RefinedOrbitCamera>().FromInstance(_camera).AsSingle();
            Container.Bind<UserInput>().FromInstance(_userInput).AsSingle();
            Container.Bind<UserInputHandler>().FromInstance(_userInputHandler).AsSingle();
        }

        private void BindScriptableObjects()
        {
            //Container.Bind<PlayerSettingsScriptableObject>().FromScriptableObject(_playerSettings).AsSingle();
            //Container.Bind<ColorStateSettingsScriptableObject>().FromScriptableObject(_colorStateSettings).AsSingle();
            //Container.Bind<LevelSettingsScriptableObject>().FromScriptableObject(_levelSettings).AsSingle();
        }

        private void BindUi()
        {
            Container.Bind<UiButtonController>().AsSingle();
            Container.Bind<UiSceneController>().FromInstance(_uiSceneController).AsSingle();
            Container.Bind<UiSceneView>().FromInstance(_uiSceneView).AsSingle();
        }

        private void SubscribeEvents()
        {
            _stateMachine.OnTransitionStart += _graph.ResetVertices;
            _stateMachine.OnTransitionStart += _stateMachine.StartTransition;
            
            _stateMachine.OnTransitionComplete += () => _verticesBlocker.Block();
            _stateMachine.OnTransitionComplete += _graph.Load;
            _stateMachine.OnTransitionComplete += _vertexFollower.Unlink;
            _stateMachine.OnTransitionComplete += _stateMachine.CompleteTransition;


            _stateMachine.OnTransitionComplete += _uiSceneController.UpdateLabel;
        }

        private void UnsubscribeEvents()
        {
            _stateMachine.OnTransitionStart -= _graph.ResetVertices;
            _stateMachine.OnTransitionStart -= _stateMachine.StartTransition;

            _stateMachine.OnTransitionComplete -= () => _verticesBlocker.Block();
            _stateMachine.OnTransitionComplete -= _graph.Load;
            _stateMachine.OnTransitionComplete -= _vertexFollower.Unlink;
            _stateMachine.OnTransitionComplete -= _stateMachine.CompleteTransition;


            _stateMachine.OnTransitionComplete -= _uiSceneController.UpdateLabel;
        }
    }
}