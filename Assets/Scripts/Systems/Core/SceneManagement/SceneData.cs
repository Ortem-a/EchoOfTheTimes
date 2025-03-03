using Eflatun.SceneReference;

namespace Systems.Core.SceneManagement
{
    [System.Serializable]
    public class SceneData
    {
        public SceneReference Reference;
        public string Name => Reference.Name;
    }
}