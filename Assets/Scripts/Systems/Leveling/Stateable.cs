using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Systems.Movement;
using Systems.Tools;
using UnityEngine;

namespace Systems.Leveling
{
    [RequireComponent(typeof(MarkerParent))]
    public class Stateable : MonoBehaviour, IStateable
    {
        private Dictionary<int, StateOption> _options;

        public Dictionary<int, StateOption> Options => _options;

        public StateableSerializableDictionary States;

        //private BridgeService _bridgeService;

        private Sequence _sequence;
        private Coroutine _coroutine;

        private Vertex[] _vertices;

        //[SerializeField]
        //private StateOption[] _options = Array.Empty<StateOption>();

        //public StateOption[] Options => _options;

        public void Initialize()
        {
            _options = States.ToDictionary();

            _vertices = GetComponentsInChildren<Vertex>(includeInactive: true);
            //_bridgeService = GetComponent<BridgeService>();
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

            //if (Options.ContainsKey(stateId))
            //{
            //    Options[stateId] = newOption;
            //}
            //else
            //{
            //    Options.Add(stateId, newOption);
            //}

            //if (Options.Length != 0 && stateId < Options.Length)
            //{
            //    Options[stateId] = newOption;
            //}
            //else
            //{
            //    int newSize = Options.Length + (stateId - Options.Length + 1);
            //    var newOptions = new StateOption[newSize];
            //    Array.Copy(Options, newOptions, Options.Length);

            //    // fill with defaults
            //    int fromIndex = Options.Length;
            //    int length = stateId - Options.Length;
            //    for (int i = 0; i < length; i++)
            //    {
            //        newOptions[fromIndex + i] = new StateOption()
            //        {
            //            Target = target,
            //            LocalPosition = Vector3.zero,
            //            LocalRotation = Quaternion.identity,
            //            LocalScale = Vector3.one
            //        };
            //    }

            //    newOptions[stateId] = newOption;
            //    _options = newOptions;
            //}
        }

        public bool TryGetOption(int stateId, out StateOption option)
        {
            if (States.TryGetValue(stateId, out var newOption))
            {
                option = newOption;
                return true;
            }

            //if (Options.Length != 0 && stateId < Options.Length)
            //{
            //    option = Options[stateId];
            //    return true;
            //}

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