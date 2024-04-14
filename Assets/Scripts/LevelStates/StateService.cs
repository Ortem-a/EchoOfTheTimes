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
        private ColorStateSettingsScriptableObject _colorStateSettings;

        // for whole level state
        private int _completedCallbackCounter;
        private int _callbackCounter;
        private TweenCallback _onCompleteCallback;

        // for each state parameter
        private int _defaultCompleteCounter = 0;
        private int _specialCompleteCounter = 0;
        private int _completeChecker = 3;
        private TweenCallback _onComplete;

        [Inject]
        public StateService(LevelSettingsScriptableObject levelSettings, ColorStateSettingsScriptableObject colorStateSettings)
        {
            _timeToChangeState_sec = levelSettings.TimeToChangeState_sec;
            _colorStateSettings = colorStateSettings;
        }

        public void SwitchState(LevelState levelState, List<StateParameter> parameters, bool isDebug = false, TweenCallback onComplete = null)
        {
            List<Transform> acceptedTargets = null;

            _onCompleteCallback = onComplete;
            _completedCallbackCounter = 0;
            _callbackCounter = 0;

            if (parameters != null && parameters.Count != 0)
            {
                acceptedTargets = new List<Transform>();

                _callbackCounter += parameters.Count;

                foreach (var param in parameters)
                {
                    acceptedTargets.Add(param.Target);

                    AcceptState(null, param, isDebug, IncrementCallbackCounter);
                }
            }

            if (levelState.StatesParameters != null)
            {
                _callbackCounter += levelState.StatesParameters.Count;

                for (int i = 0; i < levelState.StatesParameters.Count; i++)
                {
                    if (parameters != null && parameters.Count != 0)
                    {
                        if (!acceptedTargets.Contains(levelState.StatesParameters[i].Target))
                            AcceptState(levelState.StatesParameters[i], isDebug: isDebug, onComplete: IncrementCallbackCounter);
                    }
                    else
                    {
                        AcceptState(levelState.StatesParameters[i], isDebug: isDebug, onComplete: IncrementCallbackCounter);
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

        public void AcceptState(StateParameter parameter, StateParameter specialParameter = null, bool isDebug = false, TweenCallback onComplete = null)
        {
            _onComplete = onComplete;

            if (specialParameter != null)
            {
                _specialCompleteCounter = 0;
                SpecialBehaiour(specialParameter, isDebug);
            }
            else
            {
                _defaultCompleteCounter = 0;
                DefaultBehaviour(parameter, isDebug);
            }
        }

        private void DefaultBehaviour(StateParameter parameter, bool isDebug)
        {
            if (!isDebug)
            {
                parameter.Target.DOMove(parameter.Position, _timeToChangeState_sec)
                    .OnComplete(() => OnCompleteDefaultTransformation());
                parameter.Target.DORotate(parameter.Rotation, _timeToChangeState_sec)
                    .OnComplete(() => OnCompleteDefaultTransformation());
                parameter.Target.DOScale(parameter.LocalScale, _timeToChangeState_sec)
                    .OnComplete(() => OnCompleteDefaultTransformation());
            }
            else
            {
                parameter.Target.SetPositionAndRotation(parameter.Position, Quaternion.Euler(parameter.Rotation));
                parameter.Target.localScale = parameter.LocalScale;
            }
        }

        private void SpecialBehaiour(StateParameter stateParameter, bool isDebug)
        {
            if (!isDebug)
            {
                stateParameter.Target.DOMove(stateParameter.Position, _timeToChangeState_sec)
                    .OnComplete(() => OnCompleteSpecialTransformation());
                stateParameter.Target.DORotate(stateParameter.Rotation, _timeToChangeState_sec)
                    .OnComplete(() => OnCompleteSpecialTransformation());
                stateParameter.Target.DOScale(stateParameter.LocalScale, _timeToChangeState_sec)
                    .OnComplete(() => OnCompleteSpecialTransformation());
            }
            else
            {
                stateParameter.Target.SetPositionAndRotation(stateParameter.Position, Quaternion.Euler(stateParameter.Rotation));
                stateParameter.Target.localScale = stateParameter.LocalScale;
            }
        }

        private void OnCompleteDefaultTransformation()
        {
            _defaultCompleteCounter++;

            if (_defaultCompleteCounter == _completeChecker)
            {
                _onComplete?.Invoke();
            }
        }

        private void OnCompleteSpecialTransformation()
        {
            _specialCompleteCounter++;

            if (_specialCompleteCounter == _completeChecker)
            {
                _onComplete?.Invoke();
            }
        }
    }
}