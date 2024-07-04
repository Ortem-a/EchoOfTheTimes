using EchoOfTheTimes.ScriptableObjects.Player;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Effects
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        private enum SoundType
        {
            OnFlat,
            OnStairs,
            OnLadder
        }

        private AudioSource _audioSource;
        private Dictionary<SoundType, AudioClip> _playerSounds;

        [Inject]
        private void Construct(PlayerSoundsContainerScriptableObject playerSoundsContainer)
        {
            _playerSounds = new Dictionary<SoundType, AudioClip>
            {
                { SoundType.OnFlat, playerSoundsContainer.WalkingOnFlat },
                { SoundType.OnStairs, playerSoundsContainer.WalkingOnStairs },
                { SoundType.OnLadder, playerSoundsContainer.WalkingOnLadder }
            };

            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayWalkingOnFlat() => PlaySound(SoundType.OnFlat);
        public void PlayWalkingOnStairs() => PlaySound(SoundType.OnStairs);
        public void PlayWalkingOnLadder() => PlaySound(SoundType.OnLadder);

        private void PlaySound(SoundType sound)
        {
            _audioSource.clip = _playerSounds[sound];
            _audioSource.Play();
        }
    }
}