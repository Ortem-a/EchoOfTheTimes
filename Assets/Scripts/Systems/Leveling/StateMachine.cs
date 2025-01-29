using System.Collections.Generic;
using UnityEngine;

namespace Systems.Leveling
{
    public class StateMachine
    {
        private Dictionary<int, List<IStateable>> _states;

        private StateService _stateService;

        private int _currentState = 0;

        public int StatesNumber { get; private set; }

        public StateMachine(StateService stateService)
        {
            _stateService = stateService;

            var stateables = new List<IStateable>();
            foreach (var item in Object.FindObjectsOfType<Stateable>())
            {
                stateables.Add(item);
            }

            StatesNumber = GetStatesNumber(stateables);

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

        private int GetStatesNumber(List<IStateable> stateables)
        {
            int maxStateId = int.MinValue;
            foreach (var stateable in stateables)
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