using UnityEngine;

namespace EchoOfTheTimes.ScriptableObjects.Player
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Player/PlayerSoundsContainer", order = 2)]
    public class PlayerSoundsContainerScriptableObject : ScriptableObject
    {
        [field: SerializeField]
        public AudioClip WalkingOnFlat { get; private set; }
        [field: SerializeField]
        public AudioClip WalkingOnStairs { get; private set; }
        [field: SerializeField]
        public AudioClip WalkingOnLadder { get; private set; }
    }
}