using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EchoOfTheTimes.SceneManagement
{
    [System.Serializable]
    public class GameLevel
    {
        public string FullName = "Chapter|Level";
        public string ChapterName => FullName.Split('|')[0];
        public string LevelName => FullName.Split('|')[1];

        public int Collected;

        public List<SceneData> Scenes;

        public StatusType LevelStatus = StatusType.Locked;

        public string FindSceneNameByType(SceneType type)
        {
            return Scenes.FirstOrDefault(scene => scene.Type == type)?.Reference.Name;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var scene in Scenes)
            {
                sb.Append($"\t{scene.Name}\n");
            }

            return $"Full Name: {FullName} ({LevelStatus})\n{sb}";
        }
    }
}