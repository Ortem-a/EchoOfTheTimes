using Io.AppMetrica;
using UnityEngine;
using System;

public static class AppMetricaActivator
{
    private static string uniqueId;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Activate()
    {
        // �������� ID: �������� ����������, ����� ��� ����������
        GetUniqueId();

        AppMetricaConfig config = new AppMetricaConfig("3ea92f77-fb26-430e-bbf7-51b28bb93658");

        // ������������� ID � AppMetrica
        if (!string.IsNullOrEmpty(uniqueId))
        {
            AppMetrica.SetUserProfileID(uniqueId);
            Debug.Log("AppMetrica: ���� ID���� " + uniqueId);
        }
        else
        {
            Debug.LogWarning("AppMetrica: ��� ID�����");
        }

        AppMetrica.Activate(config);
    }

    // ����� ��� ��������� ����������� ��������������
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
