using System.Collections.Generic;
using UnityEngine;

namespace Systems.Units
{
    public class AnimationService
    {
        private readonly Animator _animator;
        private readonly Dictionary<int, float> _animationsDuration;

        private const float _crossFadeDuration = 0.1f;

        private readonly int _idleHash = Animator.StringToHash("Idle");
        private readonly int _moveHash = Animator.StringToHash("Walking");
        private readonly int _climbingUpHash = Animator.StringToHash("Climbing_up");
        private readonly int _climbingDownHash = Animator.StringToHash("Climbing_down");
        private readonly int _stairsUpHash = Animator.StringToHash("Stairs_up");
        private readonly int _stairsDownHash = Animator.StringToHash("Stairs_down");

        public AnimationService(Animator animator)
        {
            _animator = animator;

            _animationsDuration = new Dictionary<int, float>()
            {
                { _idleHash, 0.1f },
                { _moveHash, 0.1f },
                { _climbingUpHash, 0.1f },
                { _climbingDownHash, 0.1f },
                { _stairsUpHash, 0.1f },
                { _stairsDownHash, 0.1f },
            };
        }

        public float Idle() => PlayAnimation(_idleHash);
        public float Move() => PlayAnimation(_moveHash);
        public float ClimbUp() => PlayAnimation(_climbingUpHash);
        public float ClimbDown() => PlayAnimation(_climbingDownHash);
        public float WalkUpStairs() => PlayAnimation(_stairsUpHash);
        public float WalkDownStairs() => PlayAnimation(_stairsDownHash);

        private float PlayAnimation(int animationHash)
        {
            _animator.CrossFade(animationHash, _crossFadeDuration);
            return _animationsDuration[animationHash];
        }
    }
}