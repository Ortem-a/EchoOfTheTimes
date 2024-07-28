using DG.Tweening;
using EchoOfTheTimes.ScriptableObjects.Level;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using EchoOfTheTimes.Units;
using System;

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

        [Inject]
        private void Construct(Player playerInstance)
        {
            player = playerInstance;
        }

        private void Start()
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                _isReadyToPlaySound = true;
                Debug.Log("LevelAudioManager is ready to play sound.");
            });
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
            if (_isReadyToPlaySound)
            {
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
            else
            {
                Debug.LogWarning($"{soundName} sound not ready to play for scene: {SceneManager.GetActiveScene().name}");
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
            foreach (var levelSound in _levelSoundsSceneContainer.LevelSounds)
            {
                if (levelSound.LevelScene.name == sceneName)
                {
                    return levelSound;
                }
            }
            return null;
        }
    }
}
