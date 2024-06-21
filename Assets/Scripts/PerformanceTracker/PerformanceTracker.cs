using UnityEngine;
using UnityEngine.Analytics;
using System.Collections.Generic;
using System.Linq;

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

    void Update()
    {
        float deltaTime = Time.unscaledDeltaTime;
        float fps = 1.0f / deltaTime;
        fpsList.Add(fps);

        // Для отладки: вызываем SendPerformanceData на клавишу T
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Manual test call to SendPerformanceData");
            SendPerformanceData("ManualTestScene");
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

            AnalyticsResult result = Analytics.CustomEvent("levelPerformanceData", new Dictionary<string, object>
            {
                { "level", sceneName },
                { "duration", elapsedTime },
                { "minFps", minFps },
                { "maxFps", maxFps },
                { "avgFps", avgFps },
                { "targetFps", Application.targetFrameRate },
                { "deviceModel", SystemInfo.deviceModel },
                { "deviceType", SystemInfo.deviceType.ToString() },
                { "operatingSystem", SystemInfo.operatingSystem },
                { "isEditor", isEditor }
            });

            Debug.Log($"AnalyticsResult: {result}");
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