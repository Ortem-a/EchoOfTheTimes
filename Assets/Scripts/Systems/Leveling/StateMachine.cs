using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Systems.Leveling
{
    public class StateMachine
    {
        private readonly Dictionary<int, List<IStateable>> _states;

        public int StatesNumber { get; private set; }

        public StateMachine()
        {
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
                    if (stateables[i].Options.ContainsKey(j))
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
                var max = stateable.Options.Keys.Max();

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
            var options = _states[stateId];

            for (int i = 0; i < options.Count; i++)
            {
                options[i].AcceptState(stateId);
            }
        }
    }
}