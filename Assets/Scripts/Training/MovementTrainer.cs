using DG.Tweening;
using EchoOfTheTimes.Units;
using UnityEngine;

namespace EchoOfTheTimes.Training
{
    public class MovementTrainer : MechanicTrainer
    {
        [SerializeField]
        private Player _player;

        private bool _isHidden;
        private bool _isShown;

        [SerializeField]
        private GameObject _hintPrefab;

        [SerializeField]
        private float _fadingDuration_sec;
        [SerializeField]
        private float _afkDuration_sec;

        private DumbTimer _timer;

        private void Awake()
        {
            _hintPrefab.SetActive(false);

            Show();

            _timer = new DumbTimer(_afkDuration_sec, Show);
        }

        private void Update()
        {
            if (_player.IsBusy)
            {
                if (!_isHidden)
                {
                    Hide();

                    _timer.Reset();
                }
            }
            else
            {
                if (_isShown)
                {
                    _timer.Reset();
                }
                else
                {
                    _timer.IncrementOrComplete(Time.deltaTime);
                }
            }
        }

        public void Show()
        {
            _isShown = true;
            _isHidden = false;

            _hintPrefab.SetActive(true);

            _hintPrefab.transform.DOScale(1f, _fadingDuration_sec);
        }

        public void Hide()
        {
            _isHidden = true;
            _isShown = false;

            _hintPrefab.transform.DOScale(0f, _fadingDuration_sec)
                .OnComplete(() => _hintPrefab.SetActive(false));
        }

        public override void Disable()
        {
            _timer.Stop();
            Hide();
        }

        public override void Activate()
        {
            throw new System.NotImplementedException();
        }
    }
}