using EchoOfTheTimes.Persistence;
using EchoOfTheTimes.ScriptableObjects.Level;
using EchoOfTheTimes.Units;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace EchoOfTheTimes.Effects
{
    [RequireComponent(typeof(AudioSource))]
    public class LevelAudioManager : MonoBehaviour
    {
        private LevelSoundsSceneContainerScriptableObject _levelSoundsSceneContainer;
        private bool _isReadyToPlaySound = false;
        private AudioSource _audioSource;
        private Player _player;
        private AudioSource _ambientAudioSource;

        private bool _isMuted;

        [Inject]
        private void Construct(Player playerInstance, LevelSoundsSceneContainerScriptableObject levelSoundsSceneContainer)
        {
            _player = playerInstance;
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

        public void PlayAmbientSound(string sceneName)
        {
            var levelSound = GetLevelSound(sceneName);
            if (levelSound != null && levelSound.AmbientSound != null)
            {
                Debug.Log($"Found ambient sound for scene: {sceneName}, sound: {levelSound.AmbientSound.name}");
                _ambientAudioSource.clip = levelSound.AmbientSound;
                _ambientAudioSource.volume = 0f;
                _ambientAudioSource.Play();
                StartCoroutine(FadeIn(_ambientAudioSource, levelSound.AmbientSoundVolume, 1f)); // Время появления эмбиент-звука
                Debug.Log("Ambient sound started.");
            }
            else
            {
                Debug.LogWarning($"No ambient sound found for scene: {sceneName}");
            }
        }

        public void StopAmbientSound()
        {
            if (_ambientAudioSource != null && _ambientAudioSource.isPlaying)
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
            PlayLevelSound(levelSound => levelSound.ChangeStateSound, levelSound => levelSound.ChangeStateSoundVolume, "Change State");
        }

        public void PlayButtonPilinkSound()
        {
            PlayLevelSound(levelSound => levelSound.LevelButtonPilinkSound, levelSound => levelSound.LevelButtonPilinkSoundVolume, "Button Pilink");
        }

        public void PlayButtonChangeSound()
        {
            PlayLevelSound(levelSound => levelSound.LevelButtonChangeSound, levelSound => levelSound.LevelButtonChangeSoundVolume, "Button Change");
        }

        public void PlayTeleportSound()
        {
            PlayLevelSound(levelSound => levelSound.TeleportSound, levelSound => levelSound.TeleportSoundVolume, "Teleport");
        }

        private void PlayLevelSound(Func<LevelSoundsSceneContainerScriptableObject.LevelSound, AudioClip> getClip, Func<LevelSoundsSceneContainerScriptableObject.LevelSound, float> getVolume, string soundName)
        {
            if (!_isReadyToPlaySound)
            {
                //Debug.LogWarning($"{soundName} sound not ready to play for scene: {SceneManager.GetActiveScene().name}");
                return;
            }

            string currentSceneName = SceneManager.GetActiveScene().name;
            var levelSound = GetLevelSound(currentSceneName);
            if (levelSound != null)
            {
                var clip = getClip(levelSound);
                var volume = getVolume(levelSound);
                if (clip != null)
                {
                    PlaySound(clip, volume);
                    Debug.Log($"{soundName} sound played.");
                }
                else
                {
                    Debug.LogWarning($"No {soundName} sound assigned for scene: {currentSceneName}");
                }
            }
            else
            {
                Debug.LogWarning($"No sound configuration found for scene: {currentSceneName}");
            }
        }

        private void PlaySound(AudioClip clip, float volume)
        {
            GameObject tempAudioGO = new GameObject("TempAudio");
            tempAudioGO.transform.SetParent(_player.transform);
            tempAudioGO.transform.localPosition = Vector3.zero;

            _audioSource = tempAudioGO.AddComponent<AudioSource>();
            _audioSource.clip = clip;
            _audioSource.volume = volume;
            _audioSource.spatialBlend = 1.0f;
            _audioSource.Play();

            Destroy(tempAudioGO, clip.length);
        }

        private LevelSoundsSceneContainerScriptableObject.LevelSound GetLevelSound(string sceneName)
        {
            return _levelSoundsSceneContainer.LevelSounds
                .FirstOrDefault(levelSound => levelSound.LevelScene.SceneName == sceneName);
        }
    }
}
