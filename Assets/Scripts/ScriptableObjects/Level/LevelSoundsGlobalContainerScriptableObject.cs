using UnityEngine;

namespace EchoOfTheTimes.ScriptableObjects.Level
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Level/LevelSoundsGlobalContainer", order = 2)]
    public class LevelSoundsGlobalContainerScriptableObject : ScriptableObject
    {
        [field: SerializeField]
        public AudioClip TeleportSound { get; private set; }
        [field: SerializeField]
        public AudioClip LevelButtonSound { get; private set; }
    }
}