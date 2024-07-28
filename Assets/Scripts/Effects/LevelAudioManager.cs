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
        private bool _isReadyToPlaySound = false; // ���� ��� ������������ ���������� � ��������������� �����

        private void Start()
        {
            // ��������� �������� � ���� ������� ����� ���, ��� ����� ������ ����������������
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
                    Debug.Log($"Playing sound for scene: {currentSceneName}"); // �������
                    AudioSource.PlayClipAtPoint(levelSound.ChangeStateSound, Vector3.zero); // ���������, ��� � ��� ���� ���������� �������� ���������������
                }
                else
                {
                    Debug.LogWarning($"No sound found for scene: {currentSceneName}"); // �������
                }
            }
            else
            {
                Debug.LogWarning($"Sound not ready to play for scene: {SceneManager.GetActiveScene().name}"); // �������
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
