using System;
using System.Collections.Generic;

namespace Systems.Leveling
{
    public class StateMachine
    {
        private Dictionary<int, List<IStateable>> _states;

        public StateMachine(int statesNumber, List<IStateable> stateables)
        {
            _states = new Dictionary<int, List<IStateable>>();

            for (int i = 0; i < statesNumber; i++)
            {
                _states.Add(i, new List<IStateable>());
            }

            for (int i = 0; i < stateables.Count; i++)
            {
                for (int j = 0; j < statesNumber; j++)
                {
                    if (stateables[i].Options.ContainsKey(j))
                    {
                        _states[j].Add(stateables[i]);
                    }
                }
            }
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