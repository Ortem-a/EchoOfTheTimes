using System;
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
            var newState = GetState(isMoving, direction);

            if (newState == _unitState) return;

            _unitState = newState;

            PlayAnimation();
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

        private void PlayAnimation()
        {
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