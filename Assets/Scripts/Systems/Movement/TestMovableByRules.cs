using DG.Tweening;
using System.Collections.Generic;
using Systems.Leveling;
using UnityEngine;

namespace Systems.Movement
{
    [RequireComponent(typeof(MarkerParent), typeof(BridgeService))]
    public class TestMovableByRules : MonoBehaviour
    {
        private BridgeService _bridgeService;

        [SerializeField]
        private float _moveDuration_sec;

        public List<Rule> Rules;

        private Vertex[] _vertices;

        private Sequence _sequence;

        private void Awake()
        {
            _bridgeService = GetComponent<BridgeService>();

            _vertices = GetComponentsInChildren<Vertex>(includeInactive: true);

            Configure();
        }

        private void OnDestroy()
        {
            _sequence.Kill();
        }

        private void Configure()
        {
            _sequence = DOTween.Sequence(transform);

            for (int i = 0; i < Rules.Count; i++)
            {
                _sequence.Join(transform.DOLocalMove(Rules[i].Option.LocalPosition, _moveDuration_sec));
                _sequence.Join(transform.DOLocalRotateQuaternion(Rules[i].Option.LocalRotation, _moveDuration_sec));
                _sequence.Join(transform.DOScale(Rules[i].Option.LocalScale, _moveDuration_sec));

                _sequence.AppendCallback(HandleIncomeInRule);

                _sequence.AppendInterval(Rules[i].StayInDuration_sec);

                _sequence.AppendCallback(HandleLeaveFromRule);
            }

            _sequence.SetLoops(-1);
            _sequence.SetEase(Ease.Linear);
        }

        private void HandleIncomeInRule()
        {
            MarkVerticesAs(false);

            _bridgeService.Connect();
        }

        private void HandleLeaveFromRule()
        {
            MarkVerticesAs(true);

            _bridgeService.Disconnect();
        }

        private void MarkVerticesAs(bool isMoving)
        {
            for (int i = 0; i < _vertices.Length; i++)
            {
                _vertices[i].IsMoving = isMoving;
            }
        }
    }
}