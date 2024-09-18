using System.Collections.Generic;
using System.Text;

namespace EchoOfTheTimes.SceneManagement
{
    [System.Serializable]
    public class GameChapter
    {
        public string Title = "Sample Chapter Title";

        public List<GameLevel> Levels;

        public StatusType ChapterStatus = StatusType.Locked; 

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (GameLevel level in Levels)
            {
                sb.Append($"\t{level}\n");
            }

            return $"Title: {Title} ({ChapterStatus})\n{sb}";
        }
    }
}