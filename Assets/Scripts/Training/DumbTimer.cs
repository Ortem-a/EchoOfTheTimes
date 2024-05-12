namespace EchoOfTheTimes.Training
{
    public class DumbTimer
    {
        private float _duration;
        private System.Action OnComplete;
        private float _counter;

        private bool _isStop;

        public DumbTimer(float duration, System.Action onComplete)
        {
            _duration = duration;
            OnComplete = onComplete;
            _counter = 0;
            _isStop = false;
        }

        public void IncrementOrComplete(float step)
        {
            if (_isStop) return;

            _counter += step;

            if (_counter >= _duration)
            {
                Reset();
                OnComplete?.Invoke();
            }
        }

        public void Reset()
        {
            _counter = 0;
            Continue();
        }

        public void Stop()
        {
            _isStop = true;
        }

        public void Continue()
        {
            _isStop = false;
        }
    }
}