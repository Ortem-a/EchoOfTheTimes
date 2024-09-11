using UnityEngine;

namespace EchoOfTheTimes.UI.MainMenu
{
    [RequireComponent(typeof(BoxCollider), typeof(ColumnTweener))]
    public class Column : MonoBehaviour
    {
        public bool IsRaise { get; private set; }

        public int Id { get; set; }

        private BoxCollider _collider;
        private ColumnTweener _tweener;
        private ColumnService _service;

        private Segment[] _segments;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _tweener = GetComponent<ColumnTweener>();
            _service = GetComponentInParent<ColumnService>();
            _segments = GetComponentsInChildren<Segment>();
        }

        private void Start()
        {
            for (int i = 0; i < _segments.Length; i++)
            {
                _segments[i].SetEnable(false);
            }
        }

        public void SetEnable(bool isEnable)
        {
            _collider.enabled = isEnable;
        }

        public void Raise()
        {
            NotifyService();
            IsRaise = true;

            _tweener.RaiseTween(() =>
            {
                for (int i = 0; i < _segments.Length; i++)
                {
                    _segments[i].SetEnable(true);
                }
            });
        }

        public void Fall(System.Action onComplete)
        {
            IsRaise = false;

            _tweener.FallTween(() => {
                for (int i = 0; i < _segments.Length; i++)
                {
                    _segments[i].SetEnable(false);
                }
                onComplete?.Invoke();
                });
        }

        private void NotifyService()
        {
            _service.HandleTouch(Id);
        }
    }
}