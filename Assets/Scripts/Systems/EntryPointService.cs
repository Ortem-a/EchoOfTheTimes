using Systems.Leveling;
using Systems.Movement;
using UnityEngine;
using Zenject;

namespace Systems
{
    public class EntryPointService : MonoBehaviour
    {
        private GraphVisibility _graph;

        private StateMachine _stateMachine;
        private StateService _stateService;

        private TestInputAdapter _inputAdapter;

        private DiContainer _container;

        [Inject]
        private void Construct(DiContainer container,
            StateService stateService, Spawner spawner, 
            GraphVisibility graph)
        {
            _container = container;

            _stateService = stateService;
            _graph = graph;

            InitializeStateables();
            InitializeStateableByButton();

            InitializeGraph();
            InitializeStateMachine();

            _inputAdapter = new TestInputAdapter(_stateMachine);
            _container.Bind<TestInputAdapter>().FromInstance(_inputAdapter).AsSingle();

            var player = spawner.RunSpawner();
            _inputAdapter.SetTarget(player);

            _container.Bind<TestUserInput>().AsSingle();
        }
        
        private void InitializeStateables()
        {
            var stateables = FindObjectsOfType<Stateable>();
            for (int i = 0; i < stateables.Length; i++)
            {
                stateables[i].Initialize();
            }
        }

        private void InitializeStateableByButton()
        {
            var stateables = FindObjectsOfType<StateableByButton>();
            for (int i = 0; i < stateables.Length; i++)
            {
                stateables[i].Initialize();
            }
        }

        private void InitializeGraph()
        {
            //_graph = FindObjectOfType<GraphVisibility>();
            _graph.ResetAndLoad();
        }

        private void InitializeStateMachine()
        {
            _stateMachine = new StateMachine(_stateService);

            _container.Bind<StateMachine>().FromInstance(_stateMachine).AsSingle();
        }
    }
}