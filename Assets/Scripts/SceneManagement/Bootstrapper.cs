using EchoOfTheTimes.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EchoOfTheTimes.SceneManagement
{
    public class Bootstrapper : PersistentSingleton<Bootstrapper>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
#warning ÇÄÅÑÜ ÐÀÇËÎ×ÈË FPS
            Application.targetFrameRate = 120;

            Debug.Log("Bootstrapper...");

            SceneManager.LoadSceneAsync("Bootstrapper", LoadSceneMode.Single);
        }
    }
}