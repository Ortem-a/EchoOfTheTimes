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
                // �������� ID: �������� ����������, ����� ��� ����������
                GetUniqueId();

                var myTrackerParams = MyTracker.MyTrackerParams;

                // �������� ID � CustomUserId
                myTrackerParams.CustomUserId = uniqueId;

                // ��������� ������������ �������
                var myTrackerConfig = MyTracker.MyTrackerConfig;
                myTrackerConfig.BufferingPeriod = 10;

                MyTracker.Init("54639465012691779610");
            }
        }

        // ����� ��� ��������� ����������� ��������������
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

                // ���� ������� �� ��������� � GAID ��������
                if (!isLimitAdTrackingEnabled && !string.IsNullOrEmpty(adId))
                {
                    uniqueId = adId;
                }
                else
                {
                    // ���� GAID ����������, ���������� deviceUniqueIdentifier
                    uniqueId = SystemInfo.deviceUniqueIdentifier;
                }
            }
            catch (Exception e)
            {
                // ���� ��������� ������ ��� ��������� GAID, ���������� deviceUniqueIdentifier
                Debug.LogWarning("������ ��� ��������� GAID: " + e.Message);
                uniqueId = SystemInfo.deviceUniqueIdentifier;
            }
        }
    }
}
