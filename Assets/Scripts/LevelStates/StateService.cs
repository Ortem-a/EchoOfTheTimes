using DG.Tweening;
using EchoOfTheTimes.ScriptableObjects.Level;
using EchoOfTheTimes.UI;
using System.Collections.Generic;
using Zenject;
using UnityEngine;

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
            // if (!isDebug) _hudController.DisableButtons();
            _hudController.DisableButtons();

            _onCompleteCallback = onComplete;
            _completedCallbackCounter = 0;
            _callbackCounter = 0;

            if (stateParameters != null && stateParameters.Count > 0)
            {
                _callbackCounter += stateParameters.Count;

                for (int i = 0; i < stateParameters.Count; i++)
                {
                    AcceptState(stateParameters[i], isDebug: isDebug, onComplete: IncrementCallbackCounter);
                }

                // Планируем смену тени на хорошую за 0.25 секунд до окончания смены состояний
                float shadowChangeDelay = Mathf.Max(0f, _timeToChangeState_sec - 0.25f);
                _hudController.ScheduleShadowsToGood(shadowChangeDelay);
            }
            else
            {
                // Если список stateParameters пуст или null, сразу включаем кнопки и тень
                _hudController.EnableButtonsImmediately();
            }

            //if (isDebug)
            //{
            //    for (int i = 0; i < stateParameters.Count; i++)
            //    {
            //        AcceptState(stateParameters[i], isDebug: isDebug, onComplete: IncrementCallbackCounter);
            //    }
            //}
            //else
            //{
            //    if (stateParameters != null && stateParameters.Count > 0)
            //    {
            //        _callbackCounter += stateParameters.Count;

            //        for (int i = 0; i < stateParameters.Count; i++)
            //        {
            //            AcceptState(stateParameters[i], isDebug: isDebug, onComplete: IncrementCallbackCounter);
            //        }

            //        // Планируем смену тени на хорошую за 0.25 секунд до окончания смены состояний
            //        float shadowChangeDelay = Mathf.Max(0f, _timeToChangeState_sec - 0.25f);
            //        _hudController.ScheduleShadowsToGood(shadowChangeDelay);
            //    }
            //    else
            //    {
            //        // Если список stateParameters пуст или null, сразу включаем кнопки и тень
            //        _hudController.EnableButtonsImmediately();
            //    }
            //}   
        }

        private void IncrementCallbackCounter()
        {
            _completedCallbackCounter++;

            if (_completedCallbackCounter == _callbackCounter)
            {
                // Все изменения состояний завершены

                // Планируем активацию кнопок через 0.05 секунд после окончания смены состояний
                _hudController.ScheduleEnableButtons(0.05f);

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

        public void DisableButtonsImmediately()
        {
            _hudController.DisableButtons();
        }
    }
}
