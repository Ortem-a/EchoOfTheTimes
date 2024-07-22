using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Effects
{
    [RequireComponent(typeof(Animator))]
    public class AnimationManager : MonoBehaviour
    {
        private Animator _animator;

        private static readonly int _idleHash = Animator.StringToHash("Idle");
        private static readonly int _moveHash = Animator.StringToHash("Jogging");
        private static readonly int _climbingHash = Animator.StringToHash("Climbing Ladder");
        private static readonly int _finishClimbingHash = Animator.StringToHash("Finish Climbing");
        private static readonly int _startClimbingHash = Animator.StringToHash("Start Climbing");
        private static readonly int _runnigUpHash = Animator.StringToHash("Running Up Stairs");
        private static readonly int _walkingUpHash = Animator.StringToHash("Walking UP");

        private const float _crossFadeDuration = 0.1f;

        private readonly Dictionary<int, float> _animationsDuration = new Dictionary<int, float>()
        {
            { _idleHash, 0.1f },
            { _moveHash, 0.1f },
            { _climbingHash, 0.1f },
            { _finishClimbingHash, 0.1f },
            { _startClimbingHash, 0.1f },
            { _runnigUpHash, 0.1f },
            { _walkingUpHash, 0.1f },
        };

        private void Awake() => _animator = GetComponent<Animator>();

        public float Idle() => PlayAnimation(_idleHash);
        public float Move() => PlayAnimation(_moveHash);
        public float Climb() => PlayAnimation(_climbingHash);
        public float FinishClimb() => PlayAnimation(_finishClimbingHash);
        public float StartClimb() => PlayAnimation(_startClimbingHash);
        public float RunUp() => PlayAnimation(_runnigUpHash);
        public float WalkUp() => PlayAnimation(_walkingUpHash);

        private float PlayAnimation(int animationHash)
        {
            _animator.CrossFade(animationHash, _crossFadeDuration);
            return _animationsDuration[animationHash];
        }
    }
}