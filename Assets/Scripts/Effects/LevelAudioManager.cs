using EchoOfTheTimes.ScriptableObjects.Level;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.Effects
{
    [RequireComponent(typeof(AudioSource))]
    public class LevelAudioManager : MonoBehaviour
    {
        private AudioSource _audioSource;

        private LevelSoundsGlobalContainerScriptableObject _globalLevelSounds;
        private LevelSoundsSceneContainerScriptableObject _sceneSounds;

        [Inject]
        private void Construct(LevelSoundsGlobalContainerScriptableObject globalLevelSounds, LevelSoundsSceneContainerScriptableObject sceneSounds)
        {
            _globalLevelSounds = globalLevelSounds;
            _sceneSounds = sceneSounds;

            _audioSource = GetComponent<AudioSource>();

            PlayRandomAmbient();
        }

        public void PlayRandomAmbient()
        {
            int randomIndex = Random.Range(0, _sceneSounds.AmbinentSounds.Length - 1);

            _audioSource.clip = _sceneSounds.AmbinentSounds[randomIndex];

            _audioSource.Play();
        }
    }
}