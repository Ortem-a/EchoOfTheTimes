using System.Collections.Generic;
using System.Linq;

namespace EchoOfTheTimes.SceneManagement
{
    [System.Serializable]
    public class SceneGroup
    {
        public string GroupName = "New Scene Group";
        public List<SceneData> Scenes;

        public string FindSceneNameByType(SceneType type)
        {
            return Scenes.FirstOrDefault(scene => scene.Type == type)?.Reference.Name;
        }
    }
}