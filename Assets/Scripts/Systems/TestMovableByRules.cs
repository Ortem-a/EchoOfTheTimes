using DG.Tweening;
using System.Collections.Generic;
using Systems.Leveling;
using Systems.Movement;
using UnityEngine;

namespace Systems
{
    [RequireComponent(typeof(MarkerParent), typeof(BridgeService))]
    public class TestMovableByRules : MonoBehaviour
    {
        private BridgeService _bridgeService;

        [SerializeField]
        private float _moveDuration_sec;

        public List<Rule> Rules;

        private Vertex[] _vertices;

        private void Awake()
        {
            _bridgeService = GetComponent<BridgeService>();

            _vertices = GetComponentsInChildren<Vertex>(includeInactive: true);

            Configure();
        }

        private void Configure()
        {
            var sequence = DOTween.Sequence(transform);

            for (int i = 0; i < Rules.Count; i++)
            {
                sequence.Join(Rules[i].Option.Target.DOLocalMove(Rules[i].Option.LocalPosition, _moveDuration_sec));
                sequence.Join(Rules[i].Option.Target.DOLocalRotateQuaternion(Rules[i].Option.LocalRotation, _moveDuration_sec));
                sequence.Join(Rules[i].Option.Target.DOScale(Rules[i].Option.LocalScale, _moveDuration_sec));

                sequence.AppendCallback(() => HandleIncomeInRule(i));

                sequence.AppendInterval(Rules[i].StayInDuration_sec);

                sequence.AppendCallback(HandleLeaveFromRule);
            }

            sequence.SetLoops(-1);
            sequence.SetEase(Ease.Linear);
        }

        private void HandleIncomeInRule(int ruleIndex)
        {
            MarkVerticesAs(false);

            _bridgeService.Connect(ruleIndex);
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