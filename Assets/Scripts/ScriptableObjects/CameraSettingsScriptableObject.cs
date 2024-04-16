using UnityEngine;

namespace EchoOfTheTimes.ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/CameraSettings", order = 5)]
    public class CameraSettingsScriptableObject : ScriptableObject
    {
        [field: SerializeField]
        public float Sensitivity { get; private set; }

        [field: SerializeField, Range(0f, 1f)]
        public float FocusCentering { get; private set; }

        [field: SerializeField]
        public float Distance { get; private set; }

        [field: SerializeField]
        public float ProjectionSize { get; private set; }

        [field: SerializeField]
        public float FocusRadius { get; private set; }

        [field: SerializeField]
        public Vector2 OrbitAngles { get; private set; }

        [field: SerializeField, Range(0f, 1f)]
        public float AutoRotationSpeed { get; private set; }

        [field: SerializeField]
        public float MaxAfkTime_sec { get; private set; }
    }
}
