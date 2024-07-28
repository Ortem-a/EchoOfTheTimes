using UnityEngine;

namespace EchoOfTheTimes.ScriptableObjects.Level
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Level/LevelSoundsGlobalContainer", order = 2)]
    public class LevelSoundsGlobalContainerScriptableObject : ScriptableObject
    {
        [field: SerializeField]
        public AudioClip FlatSurfaceStepsSound { get; private set; }  // Шаги по плоской поверхности
        [field: SerializeField]
        public AudioClip LadderStepsSound { get; private set; }  // Шаги по лестнице под 45°
        [field: SerializeField]
        public AudioClip VerticalLadderCrawlingSound { get; private set; }  // Ползание по вертикальной лестнице
    }
}
