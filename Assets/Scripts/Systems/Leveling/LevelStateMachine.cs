using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Systems.Leveling
{
    public class LevelStateMachine : IInitializable
    {
        private Dictionary<int, List<IStateable>> _states;

        private readonly StateService _stateService;

        private int _currentState = 0;

        public int StatesNumber { get; private set; }

        public LevelStateMachine(StateService stateService)
        {
            _stateService = stateService;

            var stateables = new List<IStateable>();
            foreach (var item in Object.FindObjectsOfType<Stateable>())
            {
                stateables.Add(item);
            }

            StatesNumber = GetStatesNumber(stateables);

            InitializeStates(stateables);
        }

        private void InitializeStates(List<IStateable> stateables)
        {
            _states = new Dictionary<int, List<IStateable>>();

            for (int i = 0; i < StatesNumber; i++)
            {
                _states.Add(i, new List<IStateable>());
            }

            for (int i = 0; i < stateables.Count; i++)
            {
                for (int j = 0; j < StatesNumber; j++)
                {
                    if (j < stateables[i].Options.Count)
                    {
                        _states[j].Add(stateables[i]);
                    }
                }
            }
        }

        public void Initialize()
        {
            _currentState = 0;

            if (!_states.ContainsKey(0))
            {
                Debug.LogWarning("There is no states with '0' state Id! Required at least one object with '0' state Id!");
                return;
            }

            foreach (IStateable stateable in _states[_currentState])
            {
                if (stateable.TryGetOption(_currentState, out StateOption option))
                {
                    option.Target.SetLocalPositionAndRotation(option.LocalPosition, option.LocalRotation);
                    option.Target.localScale = option.LocalScale;
                }
            }
        }

        private int GetStatesNumber(List<IStateable> stateables)
        {
            int maxStateId = int.MinValue;
            foreach (IStateable stateable in stateables)
            {
                var max = stateable.Options.Count - 1;

                if (max > maxStateId)
                {
                    maxStateId = max;
                }
            }
            maxStateId += 1;

            return maxStateId;
        }

        public void ChangeState(int stateId)
        {
            Debug.Log($"[State Machine] {_currentState} -> {stateId}");

            if (stateId == _currentState) return;

            _currentState = stateId;

            var options = _states[stateId];

            _stateService.AcceptState(stateId, options);
        }
    }
}