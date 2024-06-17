using UnityEngine;

namespace EchoOfTheTimes.ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/InputIndicatorSettings", order = 6)]
    public class InputIndicatorSettingsScriptableObject : ScriptableObject
    {
        [field: SerializeField]
        public GameObject SpherePrefab { get; private set; }
        [field: SerializeField]
        public Color DefaultSphere { get; private set; }
        [field: SerializeField]
        public Color SplashSphere { get; private set; }
        [field: SerializeField]
        public Color ErrorSphere { get; private set; }
        [field: SerializeField]
        public Color ErrorSplashSphere { get; private set; }
        [field: SerializeField]
        public float DefaultRadius { get; private set; }
        [field: SerializeField]
        public float MaxRadius { get; private set; }
        [field: SerializeField]
        public float IndicatorDuration3D_sec { get; private set; }
        [field: SerializeField]
        public float IndicatorColorDuration3D_sec { get; private set; }
    }
}