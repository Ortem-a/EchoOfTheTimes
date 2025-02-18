using System;
using System.Collections;
using UnityEngine;

namespace Systems.Units
{
    [RequireComponent(typeof(Animator))]
    public class UnitAnimator : MonoBehaviour
    {
        private enum UnitState
        {
            Idle,
            Moving,
        }

        public Action<bool, Vector3> OnMovingStateChanched;

        private AnimationService _animationService;

        [SerializeField]
        private UnitState _unitState = UnitState.Idle;

        private void Awake()
        {
            var animator = GetComponent<Animator>();
            _animationService = new AnimationService(animator);

            OnMovingStateChanched += HandleMovingStateChanched;
        }

        private void OnDestroy()
        {
            OnMovingStateChanched -= HandleMovingStateChanched;
        }

        private void HandleMovingStateChanched(bool isMoving, Vector3 direction)
        {
            var unitState = GetState(isMoving, direction);

            if (unitState == _unitState) return;

            _unitState = unitState;

            StartCoroutine(PlayAnimation());
        }

        private UnitState GetState(bool isMoving, Vector3 direction)
        {
            if (isMoving)
            {
                return UnitState.Moving;
            }
            else
            {
                return UnitState.Idle;
            }
        }

        private IEnumerator PlayAnimation()
        {
            yield return null;

            switch (_unitState)
            {
                case UnitState.Idle:
                    _animationService.Idle();
                    break;
                case UnitState.Moving:
                    _animationService.Move();
                    break;
                default:
                    throw new ArgumentException($"Unexpected {nameof(UnitState)} for {_unitState}!");
            }
        }
    }
}