using DG.Tweening;
using EchoOfTheTimes.ScriptableObjects.Level;
using UnityEditor;
using UnityEngine;
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

        public void PlayChangeStateSound(SceneAsset scene)
        {
            if (_isReadyToPlaySound)
            {
                var levelSound = GetLevelSound(scene);
                if (levelSound != null)
                {
                    AudioSource.PlayClipAtPoint(levelSound.ChangeStateSound, Vector3.zero); // ���������, ��� � ��� ���� ���������� �������� ���������������
                }
            }
        }

        private LevelSoundsSceneContainerScriptableObject.LevelSound GetLevelSound(SceneAsset scene)
        {
            foreach (var levelSound in _levelSoundsSceneContainer.LevelSounds)
            {
                if (levelSound.LevelScene == scene)
                {
                    return levelSound;
                }
            }
            return null;
        }
    }
}
