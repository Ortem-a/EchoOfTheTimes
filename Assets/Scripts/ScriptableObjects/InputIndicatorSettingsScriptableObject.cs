using UnityEngine;

namespace EchoOfTheTimes.ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/InputIndicatorSettings", order = 6)]
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
    }
}