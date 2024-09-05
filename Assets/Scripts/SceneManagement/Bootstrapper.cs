using EchoOfTheTimes.Persistence;
using EchoOfTheTimes.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EchoOfTheTimes.SceneManagement
{
    public class Bootstrapper : PersistentSingleton<Bootstrapper>
    {
        public static SaveLoadService SaveLoadService;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            Application.targetFrameRate = 120;

            Debug.Log("Enable Save/Load Service...");
            SaveLoadService = new SaveLoadService();

            Debug.Log("Bootstrapper...");
            SceneManager.LoadSceneAsync("Bootstrapper", LoadSceneMode.Single);
        }
    }
}