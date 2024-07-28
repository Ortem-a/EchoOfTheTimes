using UnityEngine;

namespace EchoOfTheTimes.ScriptableObjects.Level
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Level/LevelSoundsGlobalContainer", order = 2)]
    public class LevelSoundsGlobalContainerScriptableObject : ScriptableObject
    {
        [field: SerializeField]
        public AudioClip FlatSurfaceStepsSound { get; private set; }  // ���� �� ������� �����������
        [field: SerializeField]
        public AudioClip LadderStepsSound { get; private set; }  // ���� �� �������� ��� 45�
        [field: SerializeField]
        public AudioClip VerticalLadderCrawlingSound { get; private set; }  // �������� �� ������������ ��������
    }
}
