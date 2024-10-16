using Io.AppMetrica;
using UnityEngine;

public static class AppMetricaActivator
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Activate()
    {
        // Наш API key 3ea92f77-fb26-430e-bbf7-51b28bb93658
        AppMetrica.Activate(new AppMetricaConfig("3ea92f77-fb26-430e-bbf7-51b28bb93658")
        {
            FirstActivationAsUpdate = !IsFirstLaunch(),
        });

        Debug.Log("AppMetrica не выёбывается");
    }

    private static bool IsFirstLaunch()
    {
        // Implement logic to detect whether the app is opening for the first time.
        // For example, you can check for files (settings, databases, and so on),
        // which the app creates on its first launch.
        return true;
    }
}
