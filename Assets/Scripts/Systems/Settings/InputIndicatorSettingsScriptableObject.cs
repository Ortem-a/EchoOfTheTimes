using UnityEngine;

namespace Systems.Settings
{
    [CreateAssetMenu(
        fileName = "New Input Indicator Settings (Sctiptable Object)",
        menuName = "Settings/Input Indicator Settings", 
        order = 6)]
    public class InputIndicatorSettingsScriptableObject : ScriptableObject
    {
        [field: Header("3D Indicator")]
        [field: SerializeField]
        public GameObject Indicator3DPrefab { get; private set; }
        [field: SerializeField]
        public float DefaultRadius { get; private set; }
        [field: SerializeField]
        public float MaxRadius { get; private set; }
        [field: SerializeField]
        public float IndicatorDuration3D_sec { get; private set; }
        [field: SerializeField]
        public float IndicatorColorDuration3D_sec { get; private set; }

        [field: Header("2D Indicator")]
        [field: SerializeField]
        public GameObject Indicator2DPrefab { get; private set; }
        [field: SerializeField]
        public float IndicatorDuration2D_sec { get; private set; }
    }
}