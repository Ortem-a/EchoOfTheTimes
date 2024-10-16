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
            Logs = true, // или для чего эта ебанина ещё есть? я не понимаю какой долбоёб обдолбанный писал доки к этому говну
        });

        Debug.Log("AppMetrica не выёбывается сука и не логируется схуяли");
    }

    private static bool IsFirstLaunch()
    {
        // Implement logic to detect whether the app is opening for the first time.
        // For example, you can check for files (settings, databases, and so on),
        // which the app creates on its first launch.
        return true;
    }
}
