using UnityEngine;
using UnityEngine.Analytics;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System;

public class PerformanceTracker : MonoBehaviour
{
    public static PerformanceTracker Instance { get; private set; }

    private List<float> fpsList = new List<float>();
    private float startTime;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();
            Debug.Log("Unity Services initialized and analytics data collection started.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to initialize Unity Services: {e.Message}");
        }
    }
    public void OnSceneLoaded(string sceneName)
    {
        Debug.Log($"PerformanceTracker - Scene loaded: {sceneName}");
        startTime = Time.time;
        fpsList.Clear();
    }

    public void OnSceneUnloaded(string sceneName)
    {
        Debug.Log($"PerformanceTracker - Scene unloaded: {sceneName}");
        SendPerformanceData(sceneName);
    }

    void SendPerformanceData(string sceneName)
    {
        Debug.Log("SendPerformanceData called");
        float elapsedTime = Time.time - startTime;

        if (fpsList.Count > 0)
        {
            float minFps = GetPercentile(fpsList, 5);
            float maxFps = GetPercentile(fpsList, 95);
            float avgFps = fpsList.Average();

            bool isEditor = Application.isEditor;

            Debug.Log($"Sending performance data: Level={sceneName}, Duration={elapsedTime}, MinFPS={minFps}, MaxFPS={maxFps}, AvgFPS={avgFps}, IsEditor={isEditor}");

            var customData = new Dictionary<string, object>
            {
                { "level", sceneName },
                { "durationLevel", elapsedTime },
                { "minFps", minFps },
                { "maxFps", maxFps },
                { "avgFps", avgFps },
                { "targetFps", Application.targetFrameRate },
                { "deviceModel", SystemInfo.deviceModel },
                { "deviceType", SystemInfo.deviceType.ToString() },
                { "operatingSystem", SystemInfo.operatingSystem },
                { "screenWidth", Screen.width },
                { "screenHeight", Screen.height },
                { "isEditor", isEditor }
            };

            AnalyticsService.Instance.CustomData("levelPerformanceData", customData);
            Debug.Log("Performance data sent.");
        }
        else
        {
            Debug.LogWarning("FPS list is empty, no data to send.");
        }
    }

    float GetPercentile(List<float> sortedList, float percentile)
    {
        sortedList.Sort();
        int N = sortedList.Count;
        float n = (N - 1) * percentile / 100.0f + 1;

        if (n == 1f) return sortedList[0];
        else if (n == N) return sortedList[N - 1];
        else
        {
            int k = (int)n;
            float d = n - k;
            return sortedList[k - 1] + d * (sortedList[k] - sortedList[k - 1]);
        }
    }
}
