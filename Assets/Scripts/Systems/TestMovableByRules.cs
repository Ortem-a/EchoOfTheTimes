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

            foreach (var rule in Rules)
            {
                sequence.Join(rule.Option.Target.DOLocalMove(rule.Option.LocalPosition, _moveDuration_sec));
                sequence.Join(rule.Option.Target.DOLocalRotateQuaternion(rule.Option.LocalRotation, _moveDuration_sec));
                sequence.Join(rule.Option.Target.DOScale(rule.Option.LocalScale, _moveDuration_sec));

                sequence.AppendCallback(HandleIncomeInRule);

                sequence.AppendInterval(rule.StayInDuration_sec);

                sequence.AppendCallback(HandleLeaveFromRule);
            }

            sequence.SetLoops(-1);
            sequence.SetEase(Ease.Linear);
        }

        private void HandleIncomeInRule()
        {
            Debug.Log("INCOME IN RULE");

            MarkVerticesAs(false);

            _bridgeService.Connect();
        }

        private void HandleLeaveFromRule()
        {
            Debug.Log("LEAVE RULE");

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