using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Systems.Leveling
{
    public class StateService : MonoBehaviour
    {
        public Action OnStartChangingState;
        public Action OnCompleteChangingState;

        [Inject]
        private void Construct()
        {
            OnStartChangingState += HandleStart;
            OnCompleteChangingState += HandleComplete;
        }

        private void OnDestroy()
        {
            OnStartChangingState -= HandleStart;
            OnCompleteChangingState -= HandleComplete;
        }

#warning йюй нфхдюрэ щрс усимч опюбхкэмн ръфекнннннн
        public void AcceptState(int stateId, List<IStateable> options)
        {
            OnStartChangingState?.Invoke();

            _optionsCount = options.Count;

            for (int i = 0; i < options.Count; i++)
            {
                options[i].AcceptState(stateId, HandleStepCompleted);
            }

            //OnCompleteChangingState?.Invoke();
        }

        private int _optionsCount;
        private int _completedOptions = 0;

        private void HandleStepCompleted()
        {
            _completedOptions++;

            if (_completedOptions == _optionsCount)
            {
                _completedOptions = 0;
                OnCompleteChangingState?.Invoke();
            }
        }

        private void HandleStart()
        {
            Debug.Log("Switching state: START");
        }

        private void HandleComplete()
        {
            Debug.Log("Switching state: COMPLETE");
        }
    }
}