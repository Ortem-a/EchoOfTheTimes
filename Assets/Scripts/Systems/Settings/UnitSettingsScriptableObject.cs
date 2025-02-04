using UnityEngine;

namespace Systems.Settings
{
    [CreateAssetMenu(fileName = "New Unit Settings (Scriptable Object)", menuName = "Settings/Unit Settings", order = 5)]
    public class UnitSettingsScriptableObject : ScriptableObject
    {
        [field: SerializeField]
        public GameObject Prefab { get; private set; }
        [field: SerializeField]
        public float Speed { get; private set; }
    }
}