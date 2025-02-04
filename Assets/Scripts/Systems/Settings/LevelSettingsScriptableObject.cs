using UnityEngine;

namespace Systems.Settings
{
    [CreateAssetMenu(
        fileName = "New Level Settings (Scriptable Object)",
        menuName = "Settings/Level Settings", order = 1)]
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