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
            LadderUp,
            LadderDown,
            StairsUp,
            StairsDown
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
                float angleBetweenUp = Vector3.Angle(direction, Vector3.up);
                //Debug.LogWarning(angleBetweenUp);
                // угол между Y == 90 -- движение по плоской поверхности
                
                if (angleBetweenUp > 80f && angleBetweenUp < 100f)
                {
                    return UnitState.Moving;
                }
                else if (angleBetweenUp < 40f)
                {
                    return UnitState.LadderUp;
                }
                else if (angleBetweenUp < 70f)
                {
                    return UnitState.StairsUp;
                }
                else if (angleBetweenUp < 145f)
                {
                    return UnitState.StairsDown;
                }
                else if (angleBetweenUp < 190f)
                {
                    return UnitState.LadderDown;
                }

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
                case UnitState.LadderUp:
                    _animationService.ClimbUp();
                    break;
                case UnitState.LadderDown:
                    _animationService.ClimbDown();
                    break;
                case UnitState.StairsUp:
                    _animationService.WalkUpStairs();
                    break;
                case UnitState.StairsDown:
                    _animationService.WalkDownStairs();
                    break;
                default:
                    throw new ArgumentException($"Unexpected {nameof(UnitState)} for {_unitState}!");
            }
        }
    }
}