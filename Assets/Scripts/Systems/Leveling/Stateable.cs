using AYellowpaper.SerializedCollections;
using DG.Tweening;
using System;
using System.Collections;
using Systems.Movement;
using Systems.Settings;
using UnityEngine;
using Zenject;

namespace Systems.Leveling
{
    [RequireComponent(typeof(MarkerParent))]
    public class Stateable : MonoBehaviour, IStateable
    {
        [SerializeField]
        [SerializedDictionary("State Id", "Option")]
        private SerializedDictionary<int, StateOption> _options;

        public SerializedDictionary<int, StateOption> Options => _options;

        private Sequence _sequence;
        private Coroutine _coroutine;

        private Vertex[] _vertices;

        private float _acceptingStateDuration_sec;

        [Inject]
        private void Construct(LevelSettingsScriptableObject levelSettings)
        {
            _acceptingStateDuration_sec = levelSettings.TimeToChangeState_sec;
        }

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
            if (_options.TryGetValue(stateId, out var option))
            {
                if (_coroutine != null) StopCoroutine(_coroutine);

                _coroutine = StartCoroutine(AcceptStateCoroutine(option, onComplete));
            }
        }

        public void SetOptionsFrom(int stateId, Transform target)
        {
            var newOption = new StateOption()
            {
                LocalPosition = target.localPosition,
                LocalRotation = target.localRotation,
                LocalScale = target.localScale,
            };

            if (!_options.TryAdd(stateId, newOption))
            {
                _options[stateId] = newOption;
            }
        }

        public bool TryGetOption(int stateId, out StateOption option)
        {
            if (_options.TryGetValue(stateId, out var newOption))
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
            MarkVerticesAs(true);

            _sequence = DOTween.Sequence();

            _sequence.Join(transform.DOLocalMove(option.LocalPosition, _acceptingStateDuration_sec));
            _sequence.Join(transform.DOLocalRotateQuaternion(option.LocalRotation, _acceptingStateDuration_sec));
            _sequence.Join(transform.DOScale(option.LocalScale, _acceptingStateDuration_sec));

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