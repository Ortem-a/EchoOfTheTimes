using Systems.Leveling;
using Systems.Movement;
using UnityEngine;
using Zenject;

namespace Systems
{
    public class EntryPointService : MonoBehaviour
    {
        public Vertex StartVertex;

        private IUnit _unit;
        private GraphVisibility _graph;

        private StateMachine _stateMachine;
        private StateService _stateService;

        [Inject]
        private void Construct(IUnit unit, StateService stateService)
        {
            _unit = unit;
            _stateService = stateService;

            InitializeStateables();
            InitializeStateableByButton();

            InitializeGraph();
            InitializeStateMachine();

            InitializePlayer();
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

        private void InitializePlayer()
        {
            _unit.Spawn(StartVertex);

            //_unit.transform.position = StartVertex.transform.position;
            //_unit.CurrentWaypoint = StartVertex;
        }

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