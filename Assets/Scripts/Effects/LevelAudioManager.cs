using DG.Tweening;
using EchoOfTheTimes.ScriptableObjects.Level;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using EchoOfTheTimes.Units;

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
            if (_isReadyToPlaySound)
            {
                string currentSceneName = SceneManager.GetActiveScene().name;
                var levelSound = GetLevelSound(currentSceneName);
                if (levelSound != null)
                {
                    PlaySound(levelSound.ChangeStateSound, 1.0f);
                }
                else
                {
                    Debug.LogWarning($"No change state sound found for scene: {currentSceneName}");
                }
            }
            else
            {
                Debug.LogWarning($"Sound not ready to play for scene: {SceneManager.GetActiveScene().name}");
            }
        }

        public void PlayButtonPilinkSound()
        {
            if (_isReadyToPlaySound)
            {
                string currentSceneName = SceneManager.GetActiveScene().name;
                var levelSound = GetLevelSound(currentSceneName);
                if (levelSound != null)
                {
                    PlaySound(levelSound.LevelButtonPilinkSound, 1.0f);
                }
                else
                {
                    Debug.LogWarning($"No button pilink sound found for scene: {currentSceneName}");
                }
            }
            else
            {
                Debug.LogWarning($"Sound not ready to play for scene: {SceneManager.GetActiveScene().name}");
            }
        }

        public void PlayButtonChangeSound()
        {
            if (_isReadyToPlaySound)
            {
                string currentSceneName = SceneManager.GetActiveScene().name;
                var levelSound = GetLevelSound(currentSceneName);
                if (levelSound != null)
                {
                    PlaySound(levelSound.LevelButtonChangeSound, 1.0f);
                }
                else
                {
                    Debug.LogWarning($"No button change sound found for scene: {currentSceneName}");
                }
            }
            else
            {
                Debug.LogWarning($"Sound not ready to play for scene: {SceneManager.GetActiveScene().name}");
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
