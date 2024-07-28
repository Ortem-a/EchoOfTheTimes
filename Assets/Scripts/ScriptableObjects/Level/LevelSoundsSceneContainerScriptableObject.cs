using UnityEngine;
using UnityEditor;

namespace EchoOfTheTimes.ScriptableObjects.Level
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Level/LevelSoundsSceneContainer", order = 3)]
    public class LevelSoundsSceneContainerScriptableObject : ScriptableObject
    {
        [System.Serializable]
        public class LevelSound
        {
            public SceneAsset LevelScene;
            public AudioClip AmbientSound;
            public AudioClip ChangeStateSound;
            public AudioClip MovableByRulesObjectsSound;
            public AudioClip LevelButtonPilinkSound;
            public AudioClip LevelButtonChangeSound;
        }

        [field: SerializeField]
        public LevelSound[] LevelSounds { get; private set; }
    }
}
