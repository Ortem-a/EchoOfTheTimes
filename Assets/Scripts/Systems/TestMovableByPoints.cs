using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    public class TestMovableByPoints : MonoBehaviour
    {
        [SerializeField]
        private float _delay_sec = 2f;
        [SerializeField]
        private List<Vector3> _points;

        private void Awake()
        {
            var sequence = DOTween.Sequence(transform);
            sequence.SetDelay(_delay_sec);
            sequence.AppendInterval(_delay_sec);
            sequence.PrependInterval(_delay_sec);

            foreach (var point in _points)
            {
                sequence.Append(transform.DOMove(point, 2f).SetDelay(_delay_sec));
            }

            sequence.SetLoops(-1);
            sequence.SetEase(Ease.Linear);

            sequence = sequence.Play();
        }
    }
}