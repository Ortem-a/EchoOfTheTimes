using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Leveling
{
    public class StatesInvoker
    {
        private int _optionsCount;
        private int _completedOptions = 0;

        private Action _onStateChanged;

#warning йюй нфхдюрэ щрс усимч опюбхкэмн ръфекнннннн
        public void AcceptState(int stateId, List<IStateable> options, Action onComplete)
        {
            _onStateChanged = onComplete;
            _optionsCount = options.Count;

            for (int i = 0; i < options.Count; i++)
            {
                options[i].AcceptState(
                    stateId,
                    HandleStepCompleted);
            }
        }

        private void HandleStepCompleted()
        {
            _completedOptions++;

            if (_completedOptions == _optionsCount)
            {
                _completedOptions = 0;
                _onStateChanged?.Invoke();
            }
        }
    }
}