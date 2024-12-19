using System;
using System.Collections.Generic;

namespace Systems.Leveling
{
    public class StateMachine
    {
        private Dictionary<int, List<IStateable>> _states;

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