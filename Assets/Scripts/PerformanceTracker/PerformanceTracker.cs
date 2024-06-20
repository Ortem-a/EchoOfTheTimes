using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class PerformanceTracker : MonoBehaviour
{
    private static PerformanceTracker instance;

    // Метрики для FPS
    private List<float> fpsList = new List<float>();
    private float startTime;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        // Сбор текущего FPS
        float deltaTime = Time.unscaledDeltaTime;
        float fps = 1.0f / deltaTime;
        fpsList.Add(fps);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Начало отслеживания времени и FPS для нового уровня
        startTime = Time.time;
        fpsList.Clear(); // Очистка списка FPS для нового уровня
    }

    void OnSceneUnloaded(Scene scene)
    {
        // Отправка данных при завершении уровня
        if (!Application.isEditor)
        {
            SendPerformanceData(); // Отправляем данные только на реальных устройствах
        }
    }

    void SendPerformanceData()
    {
        string currentLevel = SceneManager.GetActiveScene().name;
        float elapsedTime = Time.time - startTime; // Длительность прохождения уровня

        if (fpsList.Count > 0)
        {
            // Исключаем худшие 5% и лучшие 5% для расчета мин. и макс. FPS
            float minFps = GetPercentile(fpsList, 5);
            float maxFps = GetPercentile(fpsList, 95);
            float avgFps = fpsList.Average();

            // Собираем и отправляем данные
            Analytics.CustomEvent("levelPerformanceData", new Dictionary<string, object>
            {
                { "level", currentLevel },
                { "duration", elapsedTime },
                { "minFps", minFps },
                { "maxFps", maxFps },
                { "avgFps", avgFps },
                { "targetFps", Application.targetFrameRate },
                { "deviceModel", SystemInfo.deviceModel },
                { "deviceType", SystemInfo.deviceType.ToString() },
                { "operatingSystem", SystemInfo.operatingSystem }
            });
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
