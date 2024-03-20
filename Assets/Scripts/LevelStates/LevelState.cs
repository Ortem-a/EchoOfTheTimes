using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    [System.Serializable]
    public class LevelState
    {
        public int Id;

        public List<StateParameter> StatesParameters;

        [NonSerialized]
        private int _completedCallbackCounter;
        [NonSerialized]
        private int _callbackCounter;
        [NonSerialized]
        private TweenCallback _onCompleteCallback;

        public void Accept(List<StateParameter> parameters, bool isDebug = false, TweenCallback onComplete = null)
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

                    param.AcceptState(param, isDebug, IncrementCallbackCounter);
                }
            }

            if (StatesParameters != null)
            {
                _callbackCounter += StatesParameters.Count;

                for (int i = 0; i < StatesParameters.Count; i++)
                {
                    if (parameters != null && parameters.Count != 0)
                    {
                        if (!acceptedTargets.Contains(StatesParameters[i].Target))
                            StatesParameters[i].AcceptState(isDebug: isDebug, onComplete: IncrementCallbackCounter);
                    }
                    else
                    {
                        StatesParameters[i].AcceptState(isDebug: isDebug, onComplete: IncrementCallbackCounter);
                    }
                }
            }
            else
            {
                Debug.LogWarning($"Can't accept Level state [{Id}] without objects!");
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

        private bool AlreadyAccepted(List<StateParameter> parameters, int stateId)
        {
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    if (param.StateId == stateId)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}