using System.Collections.Generic;

namespace EchoOfTheTimes.SceneManagement
{
    [System.Serializable]
    public class GameChapter
    {
        public string Title = "Sample Chapter Title";

        public List<GameLevel> Levels;
    }
}