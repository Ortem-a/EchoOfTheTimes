using DG.Tweening;
using UnityEngine;

namespace EchoOfTheTimes.UI.MainMenu
{
    public class ColumnTweener : MonoBehaviour
    {
        public float TopY = 3f;
        public float BottomY = 0f;
        public float Duration = 2f;
        public Ease RaiseEase = Ease.OutBounce;
        public Ease FallEase = Ease.InBounce;

        public void RaiseTween(System.Action onComplete)
        {
            transform.DOLocalMoveY(TopY, Duration)
                .OnComplete(() => onComplete?.Invoke())
                .SetEase(RaiseEase);
        }

        public void FallTween(System.Action onComplete)
        {
            transform.DOLocalMoveY(BottomY, Duration)
                .OnComplete(() => onComplete?.Invoke())
                .SetEase(FallEase);
        }
    }
}