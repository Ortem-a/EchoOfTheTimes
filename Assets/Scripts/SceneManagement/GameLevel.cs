using System.Collections.Generic;
using System.Linq;

namespace EchoOfTheTimes.SceneManagement
{
    [System.Serializable]
    public class GameLevel
    {
        public string FullName = "Chapter|Level";
        public string ChapterName => FullName.Split('|')[0];
        public string LevelName => FullName.Split('|')[1];

        public List<SceneData> Scenes;

        public bool IsLocked = true;

        public string FindSceneNameByType(SceneType type)
        {
            return Scenes.FirstOrDefault(scene => scene.Type == type)?.Reference.Name;
        }
    }
}