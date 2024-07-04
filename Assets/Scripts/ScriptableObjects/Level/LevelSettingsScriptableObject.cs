using UnityEngine;

namespace EchoOfTheTimes.ScriptableObjects.Level
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Level/LevelSettings", order = 1)]
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