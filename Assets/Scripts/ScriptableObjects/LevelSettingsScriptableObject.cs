using UnityEngine;

namespace EchoOfTheTimes.ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/LevelSettings", order = 4)]
    public class LevelSettingsScriptableObject : ScriptableObject
    {
        [field: SerializeField]
        public float TeleportDuration_sec { get; private set; }
        [field: SerializeField]
        public float TeleportDisappearDuration_sec { get; private set; }
        [field: SerializeField]
        public float TimeToChangeState_sec { get; private set; }
        [field: SerializeField]
        public float MaxDistanceToNeighbourVertex { get; private set; }
    }
}