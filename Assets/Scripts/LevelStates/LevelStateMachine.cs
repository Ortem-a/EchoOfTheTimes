using EchoOfTheTimes.Editor;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor;
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
        private SceneField _currentScene; // Добавлено

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

        //public SceneAsset CurrentScene // Добавлено
        //{
        //    get { return _currentScene; }
        //    private set { _currentScene = value; }
        //}

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
                _stateService.SwitchState(state.StatesParameters, true);
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
            Debug.Log("СЦЕНА " + " " + SceneManager.GetActiveScene().name); // Используем имя текущей активной сцены

            _stateService.SwitchState(
                stateParameters: _current.StatesParameters,
                isDebug: isDebug,
                onComplete: () => OnTransitionComplete?.Invoke());
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
                            //Scene = state.Scene // Установка Scene из StateParameter
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
