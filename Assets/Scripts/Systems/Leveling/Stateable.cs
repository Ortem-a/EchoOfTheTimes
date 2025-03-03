using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Systems.Movement;
using Systems.Tools;
using UnityEngine;
using Zenject;

namespace Systems.Leveling
{
    [RequireComponent(typeof(MarkerParent))]
    public class Stateable : MonoBehaviour, IStateable
    {
        private Dictionary<int, StateOption> _options;

        public Dictionary<int, StateOption> Options
        {
            get
            {
                _options ??= States.ToDictionary();

                return _options;
            }
        }

        public StateableSerializableDictionary States;

        private Sequence _sequence;
        private Coroutine _coroutine;

        private Vertex[] _vertices;

        private void Awake()
        {
            _vertices = GetComponentsInChildren<Vertex>(includeInactive: true);
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
        }

        public void AcceptState(int stateId, Action onComplete)
        {
            if (Options.TryGetValue(stateId, out var option))
            {
                if (_coroutine != null) StopCoroutine(_coroutine);

                _coroutine = StartCoroutine(AcceptStateCoroutine(option, onComplete));
            }
        }

        public void SetOptionsFrom(int stateId, Transform target)
        {
            var newOption = new StateOption()
            {
                Target = target,
                LocalPosition = target.localPosition,
                LocalRotation = target.localRotation,
                LocalScale = target.localScale,
            };

            States.AddOrUpdate(stateId, target);
        }

        public bool TryGetOption(int stateId, out StateOption option)
        {
            if (States.TryGetValue(stateId, out var newOption))
            {
                option = newOption;
                return true;
            }

            Debug.LogError($"There is no option with key: <{stateId}>");

            option = null;
            return false;
        }

        private IEnumerator AcceptStateCoroutine(StateOption option, Action onComplete)
        {
            float duration = 2f;

            MarkVerticesAs(true);

            _sequence = DOTween.Sequence();

            _sequence.Join(option.Target.DOLocalMove(option.LocalPosition, duration));
            _sequence.Join(option.Target.DOLocalRotateQuaternion(option.LocalRotation, duration));
            _sequence.Join(option.Target.DOScale(option.LocalScale, duration));

            yield return _sequence.WaitForCompletion();

            MarkVerticesAs(false);

            onComplete?.Invoke();
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