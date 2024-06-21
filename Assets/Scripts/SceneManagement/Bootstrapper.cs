using EchoOfTheTimes.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services;
using Unity.Services.Core;

namespace EchoOfTheTimes.SceneManagement
{
    public class Bootstrapper : PersistentSingleton<Bootstrapper>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            Application.targetFrameRate = 120;

            Debug.Log("Bootstrapper...");

            SceneManager.LoadSceneAsync("Bootstrapper", LoadSceneMode.Single);
        }
    }
}