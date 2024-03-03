using EchoOfTheTimes.EditorTools;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    public class LevelStateMachine : MonoBehaviour
    {
        [Space]
        [InspectorButton(nameof(InitializeStates))]
        public bool _isInitFirstState;
        [Space]
        [Space]
        [InspectorButton(nameof(AddTransitionsEveryoneWithEvery))]
        public bool _isAddTransitionsEveryoneWithEvery;
        [Space]

        public List<LevelState> States;
        public List<Transition> Transitions;

        private LevelState _current;

        private void Awake()
        {
            LoadDefaultState();
        }

        private void LoadDefaultState()
        {
            _current = States[0];
            _current.Accept(null);
        }

        public void ChangeState(LevelState newState)
        {
            ChangeState(newState.Id);
        }

        public void ChangeState(int newStateId)
        {
            Debug.Log($"[LevelStateMachine] ChangeState: {(_current != null ? _current.Id : "<null>")} -> {newStateId}");

            if (_current != null) 
            {
                if (_current.Id == newStateId) 
                {
                    return;
                }
            }

            var transicion = Transitions.Find((x) => x.StateFromId == _current.Id && x.StateToId == newStateId);

            if (transicion != null)
            {
                _current = States.Find((x) => x.Id == newStateId);
                _current.Accept(transicion.Parameters);
            }
        }

        public void InitializeStates()
        {
            var stateables = FindObjectsOfType<Stateable>();

            States = new List<LevelState>();

            foreach (var stateable in stateables) 
            {
                foreach (var state in stateable.States)
                {
                    if (!HasState(state.StateId))
                    {
                        States.Add(new LevelState() 
                        { 
                            Id = state.StateId,
                        });
                    }
                }
            }

            List<StateParameter> statesOfSimilarId;
            foreach (var state in States)
            {
                statesOfSimilarId = new List<StateParameter>();

                foreach (var stateable in stateables)
                {
                    var stateParams = stateable.States.FindAll((x) => x.StateId == state.Id);
                    statesOfSimilarId.AddRange(stateParams);
                }

                AddToStateMachine(state.Id, statesOfSimilarId);
            }
        }

        private bool HasState(int stateId)
        {
            if (States != null)
            {
                foreach (var state in States)
                {
                    if (state.Id == stateId)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void AddToStateMachine(int stateId, List<StateParameter> stateParameters)
        {
            if (States != null)
            {
                int index = States.FindIndex((x) => x.Id == stateId);
                States[index].StatesParameters = stateParameters;
            }
            else
            {
                States = new List<LevelState>()
                {
                    new LevelState()
                    {
                        Id = stateId,
                        StatesParameters = stateParameters
                    }
                };
            }
        }

        public void AddTransitionsEveryoneWithEvery()
        {
            Transitions = new List<Transition>();
            List<int> transitions = new List<int>();

            foreach (var state in States)
            {
                if (!transitions.Contains(state.Id))
                {
                    transitions.Add(state.Id);
                }
            }

            for (int i = 0;  i < transitions.Count; i++)
            {
                for (int j = 0; j < transitions.Count; j++) 
                {
                    if (i == j) continue;

                    Transitions.Add(new Transition()
                    {
                        StateFromId = i,
                        StateToId = j,
                    });
                }
            }
        }

        public void SetParamsToTransitions(List<SpecialTransition> transitions)
        {
            foreach (var transiton in transitions)
            {
                SetParamsToTransition(transiton);
            }
        }

        public void SetParamsToTransition(SpecialTransition specialTransition)
        {
            var transition = Transitions.Find((x) => x.StateFromId == specialTransition.StateFromId && x.StateToId == specialTransition.StateToId);

            if (transition != null) 
            {
                if (specialTransition.Influenced != null)
                {
                    foreach (var stateable in specialTransition.Influenced)
                    {
                        var specTrans = stateable.SpecialTransitions.Find((x) =>
                            x.StateFromId == specialTransition.StateFromId && x.StateToId == specialTransition.StateToId);

                        if (specTrans != null)
                        {
                            transition.Parameters.AddRange(specTrans.Parameters);
                        }
                        else
                        {
                            Debug.LogWarning($"There is no special transition: {specTrans.StateFromId} -> {specTrans.StateToId}");
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning($"There is no transition: {transition}");
            }
        }
    }
}