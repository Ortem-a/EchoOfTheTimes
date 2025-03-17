using System.Text;
using UnityEngine;

namespace Systems.Core.SceneManagement
{
    [CreateAssetMenu(
        fileName = "New Game Chapter",
        menuName = "Levels/Game Chapter (Scriptable Object)", order = 6)]
    public class GameChapter : ScriptableObject
    {
        [field: SerializeField]
        public string Title { get; private set; } = "Sample Chapter Title";
        [field: SerializeField]
        public GameLevel[] Levels { get; private set; }
        [field: SerializeField]
        public StatusType ChapterStatus { get; private set; } = StatusType.Locked;

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < Levels.Length; i++)
            {
                sb.Append($"\t{Levels[i]}\n");
            }

            return $"Title: {Title} ({ChapterStatus})\n{sb}";
        }
    }
}