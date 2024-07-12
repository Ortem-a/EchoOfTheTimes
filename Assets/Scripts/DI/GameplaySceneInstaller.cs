using EchoOfTheTimes.Core;
using EchoOfTheTimes.Effects;
using EchoOfTheTimes.LevelStates;
using EchoOfTheTimes.Movement;
using EchoOfTheTimes.UI;
using EchoOfTheTimes.Units;
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
        [SerializeField]
        private LevelAudioManager _levelAudioManager;

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
        private InputMediator _inputMediator;
        [SerializeField]
        private Input3DIndicator _input3DIndicator;
        [SerializeField]
        private Input2DIndicator _input2DIndicator;
        [SerializeField]
        private Movable _movable;
        [SerializeField]
        private PlayerPath _playerPath;
        [SerializeField]
        private SoundManager _soundManager;

        [Header("UI")]
        [SerializeField]
        private UiSceneController _uiSceneController;
        [SerializeField]
        private UiSceneView _uiSceneView;
        [SerializeField]
        private HUDController _hudController; // Добавляем HUDController

        public override void InstallBindings()
        {
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
            Container.Bind<StateService>().FromNew().AsSingle();

            Container.Bind<LevelStateMachine>().FromInstance(_stateMachine).AsSingle();
            Container.Bind<GraphVisibility>().FromInstance(_graph).AsSingle();
            Container.Bind<CheckpointManager>().FromInstance(_checkpointManager).AsSingle();
            Container.Bind<VerticesBlocker>().FromInstance(_verticesBlocker).AsSingle();
            Container.Bind<Input3DIndicator>().FromInstance(_input3DIndicator).AsSingle();
            Container.Bind<Input2DIndicator>().FromInstance(_input2DIndicator).AsSingle();
            Container.Bind<LevelAudioManager>().FromInstance(_levelAudioManager).AsSingle();
        }

        private void BindPlayer()
        {
            Container.Bind<VertexFollower>().FromInstance(_vertexFollower).AsSingle();
            Container.Bind<RefinedOrbitCamera>().FromInstance(_camera).AsSingle();
            Container.Bind<UserInput>().FromInstance(_userInput).AsSingle();
            Container.Bind<InputMediator>().FromInstance(_inputMediator).AsSingle();
            Container.Bind<Player>().FromInstance(_player).AsSingle();

            Container.Bind<Movable>().FromInstance(_movable).AsSingle();
            Container.Bind<PlayerPath>().FromInstance(_playerPath).AsSingle();

            Container.Bind<SoundManager>().FromInstance(_soundManager).AsSingle();
        }

        private void BindUi()
        {
            Container.Bind<UiSceneController>().FromInstance(_uiSceneController).AsSingle();
            Container.Bind<UiSceneView>().FromInstance(_uiSceneView).AsSingle();
            Container.Bind<HUDController>().FromInstance(_hudController).AsSingle(); // Регистрируем HUDController
        }

        private void SubscribeEvents()
        {
            _stateMachine.OnTransitionStart += _graph.ResetVertices;
            _stateMachine.OnTransitionStart += _stateMachine.StartTransition;
            //_stateMachine.OnTransitionStart += () => _uiSceneController.SetActiveBottomPanelImmediate(false);

            _stateMachine.OnTransitionComplete += () => _verticesBlocker.Block();
            _stateMachine.OnTransitionComplete += _graph.Load;
            _stateMachine.OnTransitionComplete += _vertexFollower.Unlink;
            _stateMachine.OnTransitionComplete += _stateMachine.CompleteTransition;

            _stateMachine.OnTransitionComplete += _uiSceneController.UpdateLabel;
            //_stateMachine.OnTransitionComplete += () => _uiSceneController.SetActiveBottomPanelImmediate(true);
        }

        private void UnsubscribeEvents()
        {
            _stateMachine.OnTransitionStart -= _graph.ResetVertices;
            _stateMachine.OnTransitionStart -= _stateMachine.StartTransition;
            //_stateMachine.OnTransitionStart -= () => _uiSceneController.SetActiveBottomPanelImmediate(false);

            _stateMachine.OnTransitionComplete -= () => _verticesBlocker.Block();
            _stateMachine.OnTransitionComplete -= _graph.Load;
            _stateMachine.OnTransitionComplete -= _vertexFollower.Unlink;
            _stateMachine.OnTransitionComplete -= _stateMachine.CompleteTransition;

            _stateMachine.OnTransitionComplete -= _uiSceneController.UpdateLabel;
            //_stateMachine.OnTransitionComplete -= () => _uiSceneController.SetActiveBottomPanelImmediate(true);
        }
    }
}
