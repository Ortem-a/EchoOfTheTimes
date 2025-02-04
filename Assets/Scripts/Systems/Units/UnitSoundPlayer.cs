using System.Collections.Generic;
using Systems.Settings;
using UnityEngine;
using Zenject;

namespace Systems.Units
{
    [RequireComponent(typeof(AudioSource))]
    public class UnitSoundPlayer : MonoBehaviour
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
        private void Construct(UnitSoundsContainerScriptableObject playerSoundsContainer)
        {
            _playerSounds = new Dictionary<SoundType, AudioClip>
            {
                { SoundType.OnFlat, playerSoundsContainer.FlatSurfaceStepSound },
                { SoundType.OnStairs, playerSoundsContainer.StairsStepSound },
                { SoundType.OnLadder, playerSoundsContainer.LadderCrawlingSound }
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