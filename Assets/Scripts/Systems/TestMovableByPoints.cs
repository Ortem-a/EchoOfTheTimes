using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    public class TestMovableByPoints : MonoBehaviour
    {
        [SerializeField]
        private List<Vector3> _points;

        private void Awake()
        {
            var sequence = DOTween.Sequence(transform);

            foreach (var point in _points)
            {
                sequence.Append(transform.DOMove(point, 2f));
            }

            sequence.SetLoops(-1);
            sequence.SetEase(Ease.Linear);

            sequence = sequence.Play();
        }
    }
}