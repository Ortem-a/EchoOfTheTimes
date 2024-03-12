using EchoOfTheTimes.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EchoOfTheTimes.SceneManagement
{
    public class Bootstrapper : PersistentSingleton<Bootstrapper>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static async void Init()
        {
            Debug.Log("Bootstrapper...");

            SceneManager.LoadSceneAsync("Bootstrapper", LoadSceneMode.Single);
        }
    }
}