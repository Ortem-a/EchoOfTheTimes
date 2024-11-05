using EchoOfTheTimes.Persistence;
using EchoOfTheTimes.ScriptableObjects.Level;
using EchoOfTheTimes.Units;
using System.Collections;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Effects
{
    [RequireComponent(typeof(AudioSource))]
    public class LevelAudioManager : MonoBehaviour
    {
        private LevelSoundsSceneContainerScriptableObject _levelSoundsSceneContainer;
        private bool _isReadyToPlaySound = false;
        private Player _player;
        private AudioSource _ambientAudioSource;
        [SerializeField]
        private AudioSource _sfxPrefab;
        private AudioSource _sfx;

        private bool _isMuted;

        [Inject]
        private void Construct(Player player, LevelSoundsSceneContainerScriptableObject levelSoundsSceneContainer)
        {
            _player = player;
            _levelSoundsSceneContainer = levelSoundsSceneContainer;

            _ambientAudioSource = GetComponent<AudioSource>();
            _ambientAudioSource.loop = true;

            _isMuted = FindObjectOfType<PersistenceService>().GetSettings();
            _ambientAudioSource.mute = _isMuted;
        }

        private void Start()
        {
            StartCoroutine(DelayedInit());
        }

        private IEnumerator DelayedInit()
        {
            yield return new WaitForSeconds(1f);
            _isReadyToPlaySound = true;
        }

        public void PlayAmbientSound()
        {
            _ambientAudioSource.clip = _levelSoundsSceneContainer.AmbientSound;
            _ambientAudioSource.volume = 0f;
            _ambientAudioSource.Play();

            StartCoroutine(FadeIn(_ambientAudioSource, _levelSoundsSceneContainer.AmbientSoundVolume, 1f));
        }

        public void StopAmbientSound()
        {
            if (_ambientAudioSource.isPlaying)
            {
                StartCoroutine(FadeOut(_ambientAudioSource, 1f)); // Время затухания эмбиент-звука
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

        public void PlayChangeStateSound()
        {
            PlayLevelSound(
                _levelSoundsSceneContainer.ChangeStateSound,
                _levelSoundsSceneContainer.ChangeStateSoundVolume
                );
        }

        public void PlayButtonPilinkSound()
        {
            PlayLevelSound(
                _levelSoundsSceneContainer.LevelButtonPilinkSound,
                _levelSoundsSceneContainer.LevelButtonPilinkSoundVolume
                );
        }

        public void PlayButtonChangeSound()
        {
            PlayLevelSound(
                _levelSoundsSceneContainer.LevelButtonChangeSound,
                _levelSoundsSceneContainer.LevelButtonChangeSoundVolume
                );
        }

        public void PlayTeleportSound()
        {
            PlayLevelSound(
                _levelSoundsSceneContainer.TeleportSound,
                _levelSoundsSceneContainer.TeleportSoundVolume
                );
        }

        private void PlayLevelSound(AudioClip clip, float volume)
        {
            if (!_isReadyToPlaySound)
            {
                return;
            }

            if (clip != null)
            {
                PlaySound(clip, volume);
            }
        }

        private void PlaySound(AudioClip clip, float volume)
        {
            if (_isMuted) return;

            var sfx = Instantiate(_sfxPrefab, _player.transform);

            sfx.clip = clip;
            sfx.mute = _isMuted;
            sfx.volume = volume;
            sfx.spatialBlend = 1f;
            sfx.Play();

            Destroy(sfx, sfx.clip.length);
        }
    }
}
