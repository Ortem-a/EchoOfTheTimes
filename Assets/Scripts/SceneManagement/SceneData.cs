using Eflatun.SceneReference;

namespace EchoOfTheTimes.SceneManagement
{
    [System.Serializable]
    public class SceneData
    {
        public SceneReference Reference;
        public string Name => Reference.Name;
        public SceneType Type;
    }
}