using DG.Tweening;
using EchoOfTheTimes.ScriptableObjects.Level;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using EchoOfTheTimes.Units; // Добавьте пространство имен, в котором находится класс Player

namespace EchoOfTheTimes.Effects
{
    [RequireComponent(typeof(AudioSource))]
    public class LevelAudioManager : MonoBehaviour
    {
        [SerializeField]
        private LevelSoundsSceneContainerScriptableObject _levelSoundsSceneContainer;
        private bool _isReadyToPlaySound = false; // Флаг для отслеживания готовности к воспроизведению звука
        private AudioSource audioSource;
        private Player player;

        [Inject]
        private void Construct(Player playerInstance)
        {
            player = playerInstance;
        }

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
                    PlaySound(levelSound.ChangeStateSound, 1.0f); // Задаем громкость
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

        private void PlaySound(AudioClip clip, float volume)
        {
            // Создаем временный объект как дочерний объект игрока
            GameObject tempAudioGO = new GameObject("TempAudio");
            tempAudioGO.transform.SetParent(player.transform); // Устанавливаем родительский объект
            tempAudioGO.transform.localPosition = Vector3.zero; // Устанавливаем локальную позицию

            audioSource = tempAudioGO.AddComponent<AudioSource>(); // Добавляем компонент AudioSource
            audioSource.clip = clip; // Устанавливаем аудиоклип
            audioSource.volume = volume; // Устанавливаем громкость
            audioSource.spatialBlend = 1.0f; // Устанавливаем пространственный звук
            audioSource.Play(); // Воспроизводим звук

            // Уничтожаем временный объект после завершения воспроизведения
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
