using Systems.Leveling;
using Systems.Movement;
using UnityEngine;
using Zenject;

namespace Systems
{
    public class EntryPointService : MonoBehaviour
    {
        //[SerializeField]
        //private UnitFactory[] _spawners;

        private GraphVisibility _graph;

        private StateMachine _stateMachine;
        private StateService _stateService;

        private TestInputAdapter _inputAdapter;
        private TestUserInput _userInput;

        [Inject]
        private void Construct(StateService stateService, TestUserInput userInput)
        {
            _stateService = stateService;
            _userInput = userInput;

            InitializeStateables();
            InitializeStateableByButton();

            InitializeGraph();
            InitializeStateMachine();

            //RunSpawners();
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

        //private void RunSpawners()
        //{
        //    for (int i = 0; i < _spawners.Length; i++)
        //    {
        //        var spawnedUnit = _spawners[i].Create();

        //        _inputAdapter = new TestInputAdapter(spawnedUnit, _stateMachine);
        //        _userInput.SetAdapter(_inputAdapter);
        //    }
        //}

        private void InitializeGraph()
        {
            _graph = FindObjectOfType<GraphVisibility>();
            _graph.ResetAndLoad();
        }

        private void InitializeStateMachine()
        {
            _stateMachine = new StateMachine(_stateService);
        }
    }
}