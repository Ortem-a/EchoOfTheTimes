using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Leveling
{
    public class StateService : IDisposable
    {
        public Action<int> OnStartChangingState;
        public Action<int> OnCompleteChangingState;

        private int _optionsCount;
        private int _completedOptions = 0;

        public StateService()
        {
            OnStartChangingState += HandleStart;
            OnCompleteChangingState += HandleComplete;
        }

        public void Dispose()
        {
            OnStartChangingState -= HandleStart;
            OnCompleteChangingState -= HandleComplete;
        }

#warning  ¿  Œ∆»ƒ¿“‹ ›“” ’”…Õﬁ œ–¿¬»À‹ÕŒ “ﬂ∆≈ÀŒŒŒŒŒŒ
        public void AcceptState(int stateId, List<IStateable> options)
        {
            OnStartChangingState?.Invoke(stateId);

            _optionsCount = options.Count;

            for (int i = 0; i < options.Count; i++)
            {
                options[i].AcceptState(
                    stateId,
                    () => HandleStepCompleted(stateId));
            }
        }

        private void HandleStepCompleted(int stateId)
        {
            _completedOptions++;

            if (_completedOptions == _optionsCount)
            {
                _completedOptions = 0;
                OnCompleteChangingState?.Invoke(stateId);
            }
        }

        private void HandleStart(int stateId)
        {
            Debug.Log($"Switching state to {stateId}: START");
        }

        private void HandleComplete(int stateId)
        {
            Debug.Log($"Switching state to {stateId}: COMPLETE");
        }
    }
}