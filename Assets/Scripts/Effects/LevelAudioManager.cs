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
        }
    }
}