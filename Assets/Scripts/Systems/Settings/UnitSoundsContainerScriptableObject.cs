using UnityEngine;

namespace Systems.Settings
{
    [CreateAssetMenu(
        fileName = "New Unit Sounds Container (Scriptable Object)",
        menuName = "Settings/Unit Sounds Container", order = 2)]
    public class UnitSoundsContainerScriptableObject : ScriptableObject
    {
        [field: SerializeField]
        [Tooltip("Шаги по плоской поверхности")]
        public AudioClip FlatSurfaceStepSound { get; private set; }

        [field: SerializeField]
        [Tooltip("Шаги по лестнице под 45°")]
        public AudioClip StairsStepSound { get; private set; }

        [field: SerializeField]
        [Tooltip("Ползание по вертикальной лестнице")]
        public AudioClip LadderCrawlingSound { get; private set; }
    }
}
