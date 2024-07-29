using UnityEngine;

namespace EchoOfTheTimes.ScriptableObjects.Level
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Level/LevelSoundsSceneContainer", order = 3)]
    public class LevelSoundsSceneContainerScriptableObject : ScriptableObject
    {
        [System.Serializable]
        public class LevelSound
        {
            public SceneField LevelScene;
            public AudioClip AmbientSound;
            public AudioClip ChangeStateSound;
            public AudioClip MovableByRulesObjectsSound;
            public AudioClip LevelButtonPilinkSound;
            public AudioClip LevelButtonChangeSound;
            public AudioClip TeleportSound;
            [Range(0f, 1f)] public float AmbientSoundVolume = 1.0f;
            [Range(0f, 1f)] public float ChangeStateSoundVolume = 1.0f;
            [Range(0f, 1f)] public float MovableByRulesObjectsSoundVolume = 1.0f;
            [Range(0f, 1f)] public float LevelButtonPilinkSoundVolume = 1.0f;
            [Range(0f, 1f)] public float LevelButtonChangeSoundVolume = 1.0f;
            [Range(0f, 1f)] public float TeleportSoundVolume = 1.0f;
        }

        [field: SerializeField]
        public LevelSound[] LevelSounds { get; private set; }
    }
}
