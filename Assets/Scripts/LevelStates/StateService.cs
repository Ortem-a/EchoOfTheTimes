using DG.Tweening;
using EchoOfTheTimes.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.LevelStates
{
    public class StateService
    {
        private float _timeToChangeState_sec;

        private int _completedCallbackCounter;
        private int _callbackCounter;
        private TweenCallback _onCompleteCallback;

        [Inject]
        public StateService(LevelSettingsScriptableObject levelSettings)
        {
            _timeToChangeState_sec = levelSettings.TimeToChangeState_sec;
        }

        public void SwitchState(LevelState levelState, List<StateParameter> transitionParameters, bool isDebug = false, TweenCallback onComplete = null)
        {
            List<Transform> acceptedTargets = null;

            _onCompleteCallback = onComplete;
            _completedCallbackCounter = 0;
            _callbackCounter = 0;

            if (transitionParameters != null && transitionParameters.Count != 0)
            {
                acceptedTargets = new List<Transform>();

                _callbackCounter += transitionParameters.Count;

                foreach (var param in transitionParameters)
                {
                    acceptedTargets.Add(param.Target);

                    AcceptState(param, isDebug: isDebug, onComplete: IncrementCallbackCounter);
                    //param.AcceptState(param, isDebug, IncrementCallbackCounter);
                }

            }

            if (levelState.StatesParameters != null)
            {
                _callbackCounter += levelState.StatesParameters.Count;

                for (int i = 0; i < levelState.StatesParameters.Count; i++)
                {
                    if (transitionParameters != null && transitionParameters.Count != 0)
                    {
                        if (!acceptedTargets.Contains(levelState.StatesParameters[i].Target))
                            AcceptState(levelState.StatesParameters[i], isDebug: isDebug,
                                onComplete: IncrementCallbackCounter);
                        //levelState.StatesParameters[i].AcceptState(isDebug: isDebug, onComplete: IncrementCallbackCounter);
                    }
                    else
                    {
                        AcceptState(levelState.StatesParameters[i], isDebug: isDebug,
                            onComplete: IncrementCallbackCounter);
                        //levelState.StatesParameters[i].AcceptState(isDebug: isDebug, onComplete: IncrementCallbackCounter);
                    }
                }
            }
            else
            {
                Debug.LogWarning($"Can't accept Level state [{levelState.Id}] without objects!");
            }
        }

        private void IncrementCallbackCounter()
        {
            _completedCallbackCounter++;

            if (_completedCallbackCounter == _callbackCounter)
            {
                _onCompleteCallback?.Invoke();
            }
        }

        public void AcceptState(StateParameter parameter,
            StateParameter specialParameter = null, bool isDebug = false,
            TweenCallback onComplete = null)
        {
            if (parameter != null)
            {
                parameter.AcceptState(
                    timeToChangeState_sec: _timeToChangeState_sec,
                    specialParameter: specialParameter,
                    isDebug: isDebug,
                    onComplete: onComplete);
            }
            else
            {
                specialParameter.AcceptState(
                    timeToChangeState_sec: _timeToChangeState_sec,
                    specialParameter: specialParameter,
                    isDebug: isDebug,
                    onComplete: onComplete);
            }
        }
    }
}