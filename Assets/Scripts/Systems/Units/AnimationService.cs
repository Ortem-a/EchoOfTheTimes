using System.Collections.Generic;
using UnityEngine;

namespace Systems.Units
{
    public class AnimationService
    {
        private readonly Animator _animator;
        private readonly Dictionary<int, float> _animationsDuration;

        private const float _crossFadeDuration = 0.01f;

        private readonly int _idleHash = Animator.StringToHash("Idle");
        private readonly int _moveHash = Animator.StringToHash("Walking");
        private readonly int _climbingHash = Animator.StringToHash("Climbing Ladder");
        private readonly int _finishClimbingHash = Animator.StringToHash("Finish Climbing");
        private readonly int _startClimbingHash = Animator.StringToHash("Start Climbing");
        private readonly int _runningUpHash = Animator.StringToHash("Running Up Stairs");
        private readonly int _walkingUpHash = Animator.StringToHash("Walking UP");

        public AnimationService(Animator animator)
        {
            _animator = animator;

            _animationsDuration = new Dictionary<int, float>()
            {
                { _idleHash, 0.1f },
                { _moveHash, 0.1f },
                { _climbingHash, 0.1f },
                { _finishClimbingHash, 0.1f },
                { _startClimbingHash, 0.1f },
                { _runningUpHash, 0.1f },
                { _walkingUpHash, 0.1f },
            };
        }

        public float Idle() => PlayAnimation(_idleHash);
        public float Move() => PlayAnimation(_moveHash);
        public float Climb() => PlayAnimation(_climbingHash);
        public float FinishClimb() => PlayAnimation(_finishClimbingHash);
        public float StartClimb() => PlayAnimation(_startClimbingHash);
        public float RunUp() => PlayAnimation(_runningUpHash);
        public float WalkUp() => PlayAnimation(_walkingUpHash);

        private float PlayAnimation(int animationHash)
        {
            _animator.CrossFade(animationHash, _crossFadeDuration);
            return _animationsDuration[animationHash];
        }
    }
}