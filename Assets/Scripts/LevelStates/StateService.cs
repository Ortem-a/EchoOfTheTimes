using DG.Tweening;
using EchoOfTheTimes.ScriptableObjects.Level;
using EchoOfTheTimes.UI;
using System.Collections.Generic;
using Zenject;

namespace EchoOfTheTimes.LevelStates
{
    public class StateService
    {
        private float _timeToChangeState_sec;
        private int _completedCallbackCounter;
        private int _callbackCounter;
        private TweenCallback _onCompleteCallback;
        private HUDController _hudController;

        [Inject]
        public StateService(LevelSettingsScriptableObject levelSettings, HUDController hudController)
        {
            _timeToChangeState_sec = levelSettings.TimeToChangeState_sec;
            _hudController = hudController;
        }

        public StateService()
        {
            _timeToChangeState_sec = 0f;
        }

        public void SwitchState(List<StateParameter> stateParameters, bool isDebug = false, TweenCallback onComplete = null)
        {
            // ������ ����� ���������
            _hudController.DisableButtons();

            _onCompleteCallback = onComplete;
            _completedCallbackCounter = 0;
            _callbackCounter = 0;

            if (stateParameters != null)
            {
                _callbackCounter += stateParameters.Count;

                for (int i = 0; i < stateParameters.Count; i++)
                {
                    AcceptState(stateParameters[i], isDebug: isDebug, onComplete: IncrementCallbackCounter);
                }
            }
        }

        private void IncrementCallbackCounter()
        {
            _completedCallbackCounter++;

            if (_completedCallbackCounter == _callbackCounter)
            {
                // ����� ����� ���������
                _hudController.EnableButtons();
                _onCompleteCallback?.Invoke();
            }
        }

        public void AcceptState(StateParameter parameter, bool isDebug = false, TweenCallback onComplete = null)
        {
            parameter.AcceptState(
                timeToChangeState_sec: _timeToChangeState_sec,
                isDebug: isDebug,
                onComplete: onComplete);
        }
    }
}
