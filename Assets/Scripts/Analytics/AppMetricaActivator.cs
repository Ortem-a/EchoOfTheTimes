using Io.AppMetrica;
using UnityEngine;
using System;

public static class AppMetricaActivator
{
    private static string uniqueId;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Activate()
    {
        // Получаем ID: пытаемся гугловский, иначе код устройства
        GetUniqueId();

        AppMetricaConfig config = new AppMetricaConfig("3ea92f77-fb26-430e-bbf7-51b28bb93658");

        // Устанавливаем ID в AppMetrica
        if (!string.IsNullOrEmpty(uniqueId))
        {
            AppMetrica.SetUserProfileID(uniqueId);
            Debug.Log("AppMetrica: есть IDшник " + uniqueId);
        }
        else
        {
            Debug.LogWarning("AppMetrica: нет IDшника");
        }

        AppMetrica.Activate(config);
    }

    // Метод для получения уникального идентификатора
    private static void GetUniqueId()
    {
        try
        {
            AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass client = new AndroidJavaClass("com.google.android.gms.ads.identifier.AdvertisingIdClient");

            AndroidJavaObject adInfo = client.CallStatic<AndroidJavaObject>("getAdvertisingIdInfo", currentActivity);
            string adId = adInfo.Call<string>("getId");
            bool isLimitAdTrackingEnabled = adInfo.Call<bool>("isLimitAdTrackingEnabled");

            // Если трекинг не ограничен и GAID доступен
            if (!isLimitAdTrackingEnabled && !string.IsNullOrEmpty(adId))
            {
                uniqueId = adId;
            }
            else
            {
                // Если GAID недоступен, используем deviceUniqueIdentifier
                uniqueId = SystemInfo.deviceUniqueIdentifier;
            }
        }
        catch (Exception e)
        {
            // Если произошла ошибка при получении GAID, используем deviceUniqueIdentifier
            Debug.LogWarning("Ошибка при получении GAID: " + e.Message);
            uniqueId = SystemInfo.deviceUniqueIdentifier;
        }
    }
}
