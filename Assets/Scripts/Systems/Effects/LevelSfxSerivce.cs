using System.Collections;
using Systems.Movement;
using Systems.Settings;
using UnityEngine;
using Zenject;

namespace Systems.Effects
{
    [RequireComponent(typeof(AudioSource))]
    public class LevelSfxSerivce : MonoBehaviour
    {
        private LevelSoundsContainerScriptableObject _levelSoundsContainer;

        private AudioSource _ambientAudioSource;
        [SerializeField]
        private AudioSource _sfxPrefab;

        private bool _isMuted;

        [Inject]
        private void Construct(LevelSoundsContainerScriptableObject levelSoundsContainer)
        {
            _levelSoundsContainer = levelSoundsContainer;

            _ambientAudioSource = GetComponent<AudioSource>();
            _ambientAudioSource.loop = true;

            _ambientAudioSource.mute = _isMuted;
        }

        public void PlayAmbientSound()
        {
            _ambientAudioSource.clip = _levelSoundsContainer.AmbientSound.Clip;
            _ambientAudioSource.volume = 0f;
            _ambientAudioSource.Play();

            StartCoroutine(FadeIn(_ambientAudioSource, _levelSoundsContainer.AmbientSound.Volume, 1f));
        }

        public void StopAmbientSound()
        {
            if (_ambientAudioSource.isPlaying)
            {
                StartCoroutine(FadeOut(_ambientAudioSource, 1f));
            }
        }

        private IEnumerator FadeIn(AudioSource audioSource, float targetVolume, float duration)
        {
            float currentTime = 0;
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(0, targetVolume, currentTime / duration);
                yield return null;
            }
            audioSource.volume = targetVolume;
        }

        private IEnumerator FadeOut(AudioSource audioSource, float duration)
        {
            float startVolume = audioSource.volume;
            float currentTime = 0;
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, 0, currentTime / duration);
                yield return null;
            }
            audioSource.volume = 0;
            audioSource.Stop();
        }

        public void PlayChangeStateSound(Vertex at)
        {
            PlaySound(
                at.transform,
                _levelSoundsContainer.ChangeStateSound
                );
        }

        public void PlayButtonPilinkSound(Vertex at)
        {
            PlaySound(
                at.transform,
                _levelSoundsContainer.LevelButtonPilinkSound
                );
        }

        public void PlayButtonChangeSound(Vertex at)
        {
            PlaySound(
                at.transform,
                _levelSoundsContainer.LevelButtonChangeSound
                );
        }

        public void PlayTeleportSound(Vertex at)
        {
            PlaySound(
                at.transform,
                _levelSoundsContainer.TeleportSound
                );
        }

        private void PlaySound(Transform at, ClipSettings settings)
        {
            if (_isMuted) return;

            var sfx = Instantiate(_sfxPrefab, at);

            sfx.clip = settings.Clip;
            sfx.mute = _isMuted;
            sfx.volume = settings.Volume;
            sfx.spatialBlend = 1f;
            sfx.Play();

            Destroy(sfx, sfx.clip.length);
        }
    }
}
