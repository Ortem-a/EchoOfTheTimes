using UnityEngine;

namespace EchoOfTheTimes.Utils
{
    public class MonoBehaviourTimer : MonoBehaviour
    {
        private float _time_sec;
        private bool _isRunning;
        private System.Action _onComplete;

        private void Update()
        {
            if (_isRunning)
            {
                _time_sec -= Time.deltaTime;

                if (_time_sec < 0)
                {
                    _isRunning = false;
                    _onComplete?.Invoke();
                }
            }
        }

        public void Run(float time_sec, System.Action onComplete)
        {
            _time_sec = time_sec;
            _onComplete = onComplete;
            _isRunning = true;
        }

        public void Stop()
        {
            _isRunning = false;
        }
    }
}