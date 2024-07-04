using UnityEngine;

namespace EchoOfTheTimes.ScriptableObjects.UI
{
    [CreateAssetMenu(menuName = "ScriptableObjects/UI/UiSoundsContainer", order = 1)]
    public class UiSoundsContainerScriptableObject : ScriptableObject
    {
        [field: SerializeField]
        public AudioClip[] BackgroundAmbient { get; private set; }
        [field: SerializeField] 
        public AudioClip OnClickButton { get; private set; }
    }
}