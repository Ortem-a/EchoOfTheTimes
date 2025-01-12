using Systems.Leveling;
using Systems.Movement;
using UnityEngine;
using Zenject;

namespace Systems
{
    public class EntryPointService : MonoBehaviour
    {
        public Vertex StartVertex;

        private Movable _movable;
        private GraphVisibility _graph;

        private StateMachine _stateMachine;
        private StateService _stateService;

        [Inject]
        private void Construct(Movable movable, StateService stateService)
        {
            _movable = movable;
            _stateService = stateService;

            InitializeStateables();

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

        private void InitializePlayer()
        {
            _movable.transform.position = StartVertex.transform.position;

            _movable.CurrentWaypoint = StartVertex;
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