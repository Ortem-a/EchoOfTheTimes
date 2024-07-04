using UnityEngine;

namespace EchoOfTheTimes.ScriptableObjects.UI
{
    [CreateAssetMenu(menuName = "ScriptableObjects/UI/SceneHudSoundsContainer", order = 2)]
    public class SceneHudSoundsContainerScriptableObject : ScriptableObject
    {
        [field: SerializeField]
        public AudioClip StartLevelSound { get; private set; }
        [field: SerializeField]
        public AudioClip FinishLevelSound { get; private set; }
        [field: SerializeField]
        public AudioClip StateButtonSound { get; private set; }
    }
}