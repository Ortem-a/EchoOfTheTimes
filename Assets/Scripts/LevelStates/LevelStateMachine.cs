using DG.Tweening;
using EchoOfTheTimes.Editor;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    public class LevelStateMachine : MonoBehaviour
    {
        [Header("DEBUG")]
        public int StateId;
        [Space]
        [InspectorButton(nameof(SetInStateDebug))]
        public bool IsDebugSetInState;
        [Header("END DEBUG")]
        [Space]


        [Space]
        [InspectorButton(nameof(InitializeStates))]
        public bool _isInitFirstState;
        [Space]
        [Space]
        [InspectorButton(nameof(AddTransitionsEveryoneWithEvery))]
        public bool _isAddTransitionsEveryoneWithEvery;
        [Space]

        private Sequence _sequence;

        public List<LevelState> States;
        public List<Transition> Transitions;

        private LevelState _current;
        public Transition LastTransition { get; private set; } = null;

        public delegate void TransitionHandler();
        public TransitionHandler OnTransitionStart;
        public TransitionHandler OnTransitionComplete;

        private void Awake()
        {
            LoadDefaultState();
        }

        private void LoadDefaultState()
        {
            _current = States[0];
            _current.Accept(null);
        }

        private void LoadStateDebug(int id)
        {
            var state = States.Find((x) => x.Id == id);

            if (state != null)
            {
                _current = state;
                _current.Accept(null, true);
            }
            else
            {
                Debug.LogWarning($"There is no state with id: {id}");
            }
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

            OnTransitionStart?.Invoke();

            var transicion = Transitions.Find((x) => x.StateFromId == _current.Id && x.StateToId == newStateId);

            if (transicion != null)
            {
                _current = States.Find((x) => x.Id == newStateId);
                _current.Accept(transicion.Parameters);

                LastTransition = transicion;
            }

            OnTransitionComplete?.Invoke();
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
                            Debug.LogWarning($"There is no special transition: {specialTransition.StateFromId}  ->  {specialTransition.StateToId}");
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning($"There is no transition: {specialTransition.StateFromId}  ->  {specialTransition.StateToId}");
            }
        }

        public void RemoveParamsFromTransitions(List<SpecialTransition> transitions)
        {
            foreach (var transiton in transitions)
            {
                RemoveParamsFromTransition(transiton);
            }
        }

        public void RemoveParamsFromTransition(SpecialTransition specialTransition)
        {
            var transition = Transitions.Find((x) => x.StateFromId == specialTransition.StateFromId && x.StateToId == specialTransition.StateToId);

            if (transition != null)
            {
                int index = Transitions.FindIndex((x) => x.StateFromId == specialTransition.StateFromId && x.StateToId == specialTransition.StateToId);

                if (specialTransition.Influenced != null)
                {
                    foreach (var stateable in specialTransition.Influenced)
                    {
                        var specTrans = stateable.SpecialTransitions.Find((x) =>
                            x.StateFromId == specialTransition.StateFromId && x.StateToId == specialTransition.StateToId);

                        if (specTrans != null)
                        {
                            foreach (var p in specTrans.Parameters)
                            {
                                Transitions[index].Parameters.Remove(p);
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"There is no special transition: {specialTransition.StateFromId} -> {specialTransition.StateToId}");
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning($"There is no transition: {specialTransition.StateFromId} -> {specialTransition.StateToId}");
            }
        }

        public void SetInStateDebug()
        {
            LoadStateDebug(StateId);
        }
    }
}