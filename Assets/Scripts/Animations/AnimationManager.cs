using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.Animations
{
    [RequireComponent(typeof(Animator))]
    public class AnimationManager : MonoBehaviour
    {
        private Animator _animator;

        private static readonly int _idleHash = Animator.StringToHash("1");
        private static readonly int _moveHash = Animator.StringToHash("222");

        private const float _crossFadeDuration = 0.1f;

        private readonly Dictionary<int, float> _animationsDuration = new Dictionary<int, float>()
        {
            { _idleHash, 0.1f },
            { _moveHash, 0.1f }
        };

        private void Awake() => _animator = GetComponent<Animator>();

        public float Idle() => PlayAnimation(_idleHash);
        public float Move() => PlayAnimation(_moveHash);

        private float PlayAnimation(int animationHash)
        {
            _animator.CrossFade(animationHash, _crossFadeDuration);
            return _animationsDuration[animationHash];
        }
    }
}