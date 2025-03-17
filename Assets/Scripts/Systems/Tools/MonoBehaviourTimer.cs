using System;
using UnityEngine;

namespace Systems.Tools
{
    public class MonoBehaviourTimer : MonoBehaviour
    {
        private float _delay_s = 0f;
        private bool _isRun = false;
        private Action _onComplete = null;

        public void Run(float delay_ms, Action onComplete)
        {
            _delay_s = delay_ms;
            _onComplete = onComplete;
            _isRun = true;
        }

        public void Stop(bool withAction = false)
        {
            _isRun = false;
            _delay_s = 0f;
            if (withAction)
            {
                _onComplete?.Invoke();
            }
            else
            {
                _onComplete = null;
            }
        }

        private void Update()
        {
            if (_isRun)
            {
                _delay_s -= Time.deltaTime;
                if (_delay_s < 0f)
                {
                    _isRun = false;
                    _onComplete?.Invoke();
                }
            }
        }
    }
}