using UnityEngine;
using System;

namespace Mycom.Tracker.Unity
{
    public class MyTrackerActivator : MonoBehaviour
    {
        private string uniqueId;

        public void Awake()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                // Получаем ID: пытаемся гугловский, иначе код устройства
                GetUniqueId();

                var myTrackerParams = MyTracker.MyTrackerParams;

                // Передаем ID в CustomUserId
                myTrackerParams.CustomUserId = uniqueId;

                // Настройка конфигурации трекера
                var myTrackerConfig = MyTracker.MyTrackerConfig;
                myTrackerConfig.BufferingPeriod = 10;

                MyTracker.Init("54639465012691779610");
            }
        }

        // Метод для получения уникального идентификатора
        private void GetUniqueId()
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
}
