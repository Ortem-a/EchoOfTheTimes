using EchoOfTheTimes.SceneManagement;
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

        public GameChapter Chapter;

        public Segment[] Segments { get; private set; }

        private Color _gizmoColor;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _tweener = GetComponent<ColumnTweener>();
            _service = GetComponentInParent<ColumnService>();
            Segments = GetComponentsInChildren<Segment>();
        }

        public void Initialize()
        {
            for (int i = 0; i < Chapter.Levels.Count; i++)
            {
                Segments[i].Level = Chapter.Levels[i];
            }

            for (int i = 0; i < Segments.Length; i++)
            {
                Segments[i].SetEnable(false);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _gizmoColor;

            Gizmos.DrawSphere(transform.position + Vector3.up, 0.5f);
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
                for (int i = 0; i < Segments.Length; i++)
                {
                    Segments[i].SetEnable(true);
                }
            });
        }

        public void Fall(System.Action onComplete)
        {
            IsRaise = false;

            _tweener.FallTween(() => {
                for (int i = 0; i < Segments.Length; i++)
                {
                    Segments[i].SetEnable(false);
                }
                onComplete?.Invoke();
                });
        }

        private void NotifyService()
        {
            _service.HandleTouch(Id);
        }

        public void MarkAs(StatusType status)
        {
            switch (status)
            {
                case StatusType.Locked:
                    MarkAsLocked();
                    break;
                case StatusType.Unlocked:
                    MarkAsUnlocked();
                    break;
                case StatusType.Completed:
                    MarkAsCompleted();
                    break;
                default:
                    throw new System.NotImplementedException($"Not implemented status '{status}'!");
            }
        }

        private void MarkAsLocked()
        {
            //Debug.Log($"Mark {name} as Locked!");

            _gizmoColor = Color.red;
        }

        private void MarkAsUnlocked()
        {
            //Debug.Log($"Mark {name} as Unlocked!");

            _gizmoColor = Color.yellow;
        }

        private void MarkAsCompleted()
        {
            //Debug.Log($"Mark {name} as Completed!");

            _gizmoColor = Color.green;
        }
    }
}