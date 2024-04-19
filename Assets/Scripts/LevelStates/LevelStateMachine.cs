using EchoOfTheTimes.Editor;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.LevelStates
{
    public class LevelStateMachine : MonoBehaviour
    {
        [Header("DEBUG")]
        public int StateId;
#if UNITY_EDITOR
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
#endif

        public List<LevelState> States;
        public List<Transition> Transitions;

        private LevelState _current;
        public Transition LastTransition { get; private set; } = null;

        public delegate void TransitionHandler();
        public TransitionHandler OnTransitionStart;
        public TransitionHandler OnTransitionComplete;

        public bool IsChanging { get; private set; }

        private StateService _stateService;

        [Inject]
        private void Construct(StateService stateService)
        {
            _stateService = stateService;
        }

        public void StartTransition()
        {
            IsChanging = true;
        }

        public void CompleteTransition()
        {
            IsChanging = false;
        }

        public void LoadState(int id, bool isDebug = false)
        {
            var state = States.Find((x) => x.Id == id);

            if (state != null)
            {
                Debug.Log($"[LevelStateMachine] LoadState: {(_current != null ? _current.Id : "<null>")} -> {id}");

                OnTransitionStart?.Invoke();

                ChangeState(state, null, isDebug);
            }
            else
            {
                Debug.LogWarning($"There is no state with id: {id}");
            }
        }

        private void LoadStateEditor(int id)
        {
            var state = States.Find((x) => x.Id == id);

            if (state != null)
            {
                _stateService = new StateService();
                _stateService.SwitchState(state.StatesParameters, null, true);
            }
            else
            {
                Debug.LogWarning($"There is no state with id: {id}");
            }
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

            var transition = Transitions.Find((x) => x.StateFromId == _current.Id && x.StateToId == newStateId);

            if (transition != null)
            {
                OnTransitionStart?.Invoke();

                var state = States.Find((x) => x.Id == newStateId);
                ChangeState(state, transition, false);
            }
        }

        private void ChangeState(LevelState state, Transition transition, bool isDebug)
        {
            _current = state;

            if (transition == null)
            {
                _stateService.SwitchState(
                    stateParameters: _current.StatesParameters, 
                    transitionParameters: null, 
                    isDebug: isDebug,
                    onComplete: () => OnTransitionComplete?.Invoke());
            }
            else
            {
                _stateService.SwitchState(
                    stateParameters: _current.StatesParameters, 
                    transitionParameters: transition.Parameters, 
                    isDebug: isDebug,
                    onComplete: () => OnTransitionComplete?.Invoke());

                LastTransition = transition;
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

            for (int i = 0; i < transitions.Count; i++)
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

        public void ChangeStateImmediate(int stateId)
        {
            LoadState(stateId, true);
        }

        public void SetInStateDebug()
        {
            LoadStateEditor(StateId);
        }

        public int GetCurrentStateId()
        {
            return _current.Id;
        }
    }
}