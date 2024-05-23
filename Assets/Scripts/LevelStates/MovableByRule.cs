using EchoOfTheTimes.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.LevelStates
{
    [RequireComponent(typeof(MonoBehaviourTimer))]
    public class MovableByRule : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header("DEBUG")]
        [SerializeField]
        private int _ruleIndex;
#endif

        [SerializeField]
        private float _timeToChangeState_sec;
        [SerializeField]
        private float _holdDelay_sec;

        [SerializeField]
        private List<StateParameter> _parameters;

        private int _parameterIndex = -1;
        private bool _isComplete = false;
        private bool _isStopped = false;

        private MonoBehaviourTimer _timer;

        private void Awake()
        {
            _timer = GetComponent<MonoBehaviourTimer>();

            Run();
        }

        private void Update()
        {
            if (_isComplete && !_isStopped)
            {
                _isComplete = false;
                _parameterIndex++;

                if (_parameterIndex == _parameters.Count)
                {
                    _parameterIndex = 0;
                }

                Move(_parameterIndex);
            }
        }

        private void Move(int parameterIndex)
        {
            _parameters[parameterIndex].AcceptState(
                onComplete: () =>
                {
                    _timer.Run(_holdDelay_sec, () => _isComplete = true);
                },
                timeToChangeState_sec: _timeToChangeState_sec);
        }

        public void Run()
        {
            _parameterIndex = -1;
            _isComplete = true;
            _isStopped = false;
        }

        public void Stop()
        {
            _timer.Stop();

            _isStopped = true;
        }

#if UNITY_EDITOR
        public void SetOrUpdateParamsToRule()
        {
            _parameters ??= new List<StateParameter>();

            if (_ruleIndex == _parameters.Count)
            {
                _parameters.Add(new StateParameter()
                {
                    StateId = -1,
                    Target = transform,
                    Position = transform.position,
                    Rotation = transform.rotation.eulerAngles,
                    LocalScale = transform.localScale
                });
            }
            else if (_ruleIndex >= 0 && _ruleIndex < _parameters.Count)
            {
                _parameters[_ruleIndex] = new StateParameter()
                {
                    StateId = -1,
                    Target = transform,
                    Position = transform.position,
                    Rotation = transform.rotation.eulerAngles,
                    LocalScale = transform.localScale
                };
            }
            else
            {
                Debug.LogError($"Incorrect Rule Index! May be [0, {_parameters.Count - 1}]");
            }
        }

        public void TransformObjectByRule()
        {
            if (_ruleIndex >= 0 && _ruleIndex < _parameters.Count)
            {
                transform.SetPositionAndRotation(
                    _parameters[_ruleIndex].Position,
                    Quaternion.Euler(_parameters[_ruleIndex].Rotation));
                transform.localScale = _parameters[_ruleIndex].LocalScale;
            }
            else
            {
                Debug.LogError($"Incorrect Rule Index! May be [0, {_parameters.Count - 1}]");
            }
        }
#endif
    }
}