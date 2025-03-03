using UnityEngine;

namespace Systems.Core.SceneManagement
{
    [CreateAssetMenu(
        fileName = "New Game Level",
        menuName = "Levels/Game Level (Scriptable Object)", order = 7)]
    public class GameLevel : ScriptableObject
    {
        [Tooltip("Sample name: 'Chapter|Level'")]
        [field: SerializeField]
        public string FullName { get; private set; } = "Chapter|Level";
        public string ChapterName => FullName.Split('|')[0];
        public string LevelName => FullName.Split('|')[1];
        [field: SerializeField]
        public int TotalCollectables { get; private set; }
        [field: SerializeField]
        public int Collected { get; private set; }
        [field: SerializeField]
        public SceneData Scene { get; private set; }
        [field: SerializeField]
        public StatusType LevelStatus { get; private set; } = StatusType.Locked;

        public override string ToString()
        {
            return $"Full Name: {FullName} ({Collected}/{TotalCollectables}) ({LevelStatus}) '{Scene.Name}'";
        }
    }
}