using UnityEngine;

namespace EchoOfTheTimes.ScriptableObjects.Level
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Level/LevelSoundsSceneContainer", order = 3)]
    public class LevelSoundsSceneContainerScriptableObject : ScriptableObject
    {
        [field: SerializeField]
        public AudioClip[] AmbinentSounds { get; private set; }
        [field: SerializeField]
        public AudioClip ChangeStateSound { get; private set; }
        [field: SerializeField]
        public AudioClip MovableByRulesObjectsSound { get; private set; }

    }
}