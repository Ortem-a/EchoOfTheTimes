using EchoOfTheTimes.ScriptableObjects.Level;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using EchoOfTheTimes.Units;
using System.Collections;
using System;
using System.Linq;

namespace EchoOfTheTimes.Effects
{
    [RequireComponent(typeof(AudioSource))]
    public class LevelAudioManager : MonoBehaviour
    {
        [SerializeField]
        private LevelSoundsSceneContainerScriptableObject _levelSoundsSceneContainer;
        private bool _isReadyToPlaySound = false;
        private AudioSource audioSource;
        private Player player;
        private AudioSource ambientAudioSource;

        [Inject]
        private void Construct(Player playerInstance)
        {
            player = playerInstance;
        }

        private void Start()
        {
            StartCoroutine(DelayedInit());

            ambientAudioSource = gameObject.AddComponent<AudioSource>();
            ambientAudioSource.loop = true;
        }

        private IEnumerator DelayedInit()
        {
            yield return new WaitForSeconds(1f);
            _isReadyToPlaySound = true;
            Debug.Log("LevelAudioManager is ready to play sound.");
        }

        public void PlayAmbientSound(string sceneName)
        {
            if (_levelSoundsSceneContainer == null)
            {
                Debug.LogWarning("LevelSoundsSceneContainerScriptableObject is not assigned.");
                return;
            }

            Debug.Log($"Attempting to play ambient sound for scene: {sceneName}");
            var levelSound = GetLevelSound(sceneName);
            if (levelSound != null && levelSound.AmbientSound != null)
            {
                Debug.Log($"Found ambient sound for scene: {sceneName}, sound: {levelSound.AmbientSound.name}");
                ambientAudioSource.clip = levelSound.AmbientSound;
                ambientAudioSource.volume = 0f;
                ambientAudioSource.Play();
                StartCoroutine(FadeIn(ambientAudioSource, levelSound.AmbientSoundVolume, 1f)); // Время появления эмбиент-звука
                Debug.Log("Ambient sound started.");
            }
            else
            {
                Debug.LogWarning($"No ambient sound found for scene: {sceneName}");
            }
        }

        public void StopAmbientSound()
        {
            if (ambientAudioSource != null && ambientAudioSource.isPlaying)
            {
                StartCoroutine(FadeOut(ambientAudioSource, 1f)); // Время затухания эмбиент-звука
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
                Debug.LogWarning($"{soundName} sound not ready to play for scene: {SceneManager.GetActiveScene().name}");
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
            if (clip == null) return;

            GameObject tempAudioGO = new GameObject("TempAudio");
            tempAudioGO.transform.SetParent(player.transform);
            tempAudioGO.transform.localPosition = Vector3.zero;

            audioSource = tempAudioGO.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.spatialBlend = 1.0f;
            audioSource.Play();

            Destroy(tempAudioGO, clip.length);
        }

        private LevelSoundsSceneContainerScriptableObject.LevelSound GetLevelSound(string sceneName)
        {
            if (_levelSoundsSceneContainer == null || _levelSoundsSceneContainer.LevelSounds == null)
            {
                Debug.LogWarning("LevelSoundsSceneContainer or its LevelSounds are not assigned.");
                return null;
            }

            return _levelSoundsSceneContainer.LevelSounds.FirstOrDefault(levelSound => levelSound.LevelScene.SceneName == sceneName);
        }
    }
}
