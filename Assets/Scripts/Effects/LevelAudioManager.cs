using DG.Tweening;
using EchoOfTheTimes.ScriptableObjects.Level;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace EchoOfTheTimes.Effects
{
    [RequireComponent(typeof(AudioSource))]
    public class LevelAudioManager : MonoBehaviour
    {
        [SerializeField]
        private LevelSoundsSceneContainerScriptableObject _levelSoundsSceneContainer;
        private bool _isReadyToPlaySound = false; // Флаг для отслеживания готовности к воспроизведению звука

        private void Start()
        {
            // Установим задержку в одну секунду перед тем, как звуки смогут воспроизводиться
            DOVirtual.DelayedCall(1f, () => _isReadyToPlaySound = true);
        }

        public void PlayChangeStateSound()
        {
            if (_isReadyToPlaySound)
            {
                string currentSceneName = SceneManager.GetActiveScene().name;
                var levelSound = GetLevelSound(currentSceneName);
                if (levelSound != null)
                {
                    Debug.Log($"Playing sound for scene: {currentSceneName}"); // Отладка
                    AudioSource.PlayClipAtPoint(levelSound.ChangeStateSound, Vector3.zero); // Убедитесь, что у вас есть подходящий источник воспроизведения
                }
                else
                {
                    Debug.LogWarning($"No sound found for scene: {currentSceneName}"); // Отладка
                }
            }
            else
            {
                Debug.LogWarning($"Sound not ready to play for scene: {SceneManager.GetActiveScene().name}"); // Отладка
            }
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
