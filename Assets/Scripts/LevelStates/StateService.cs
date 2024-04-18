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

        public void SwitchState(List<StateParameter> stateParameters, List<StateParameter> transitionParameters, bool isDebug = false, TweenCallback onComplete = null)
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

            if (stateParameters != null)
            {
                _callbackCounter += stateParameters.Count;

                for (int i = 0; i < stateParameters.Count; i++)
                {
                    if (transitionParameters != null && transitionParameters.Count != 0)
                    {
                        if (!acceptedTargets.Contains(stateParameters[i].Target))
                            AcceptState(stateParameters[i], isDebug: isDebug,
                                onComplete: IncrementCallbackCounter);
                        //levelState.StatesParameters[i].AcceptState(isDebug: isDebug, onComplete: IncrementCallbackCounter);
                    }
                    else
                    {
                        AcceptState(stateParameters[i], isDebug: isDebug,
                            onComplete: IncrementCallbackCounter);
                        //levelState.StatesParameters[i].AcceptState(isDebug: isDebug, onComplete: IncrementCallbackCounter);
                    }
                }
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