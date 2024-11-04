using UnityEngine;
using Io.AppMetrica;
using System.Collections.Generic;
using System.Linq;

namespace EchoOfTheTimes.SceneManagement
{
    public class LevelAnalyticsTracker : MonoBehaviour
    {
        private float levelStartTime;
        private float pausedTime;
        private bool isPaused = false;
        private bool isLevelTimerRunning = false;
        private bool isFPSTracking = false;

        public string levelName = "Default Level"; // Название уровня
        public string chapterName = "Default Chapter"; // Название главы
        private string status = "default"; // "default" - прошёл без сбора всех коллектаблов, "full" - собрал всё
        private int num_collectables = 0; // Количество собранных коллектаблов за это прохождение уровня

        private List<float> fpsList = new List<float>(); // Храним FPS

        private void Start()
        {
            // Автоматически начинаем сбор аналитики при запуске сцены
            StartLevelAnalytics();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            isPaused = pauseStatus;
            if (isPaused)
            {
                // Приложение свернуто
                if (isLevelTimerRunning)
                    PauseLevelTimer();
                if (isFPSTracking)
                    PauseFPSTracking();
            }
            else
            {
                // Приложение восстановлено
                if (isLevelTimerRunning)
                    ResumeLevelTimer();
                if (isFPSTracking)
                    ResumeFPSTracking();
            }
        }

        // Методы для управления таймером уровня
        private void StartLevelTimer()
        {
            if (isLevelTimerRunning)
                return;

            levelStartTime = Time.time;
            isLevelTimerRunning = true;
            Debug.Log("Level timer started");
        }

        public void EndLevelAnalytics()
        {
            EndLevelTimer();
            EndFPSTracking();
        }

        private void SendLevelStartedEvent()
        {
            string levelData = $@"
    {{
        ""level_started"": {{
            ""{levelName}"": {{}}
        }}
    }}";

            AppMetrica.ReportEvent("level_started", levelData);
            Debug.Log($"Level started event sent for {levelName}");
        }

        private void EndLevelTimer()
        {
            if (!isLevelTimerRunning)
                return;

            float levelDuration = Time.time - levelStartTime;
            isLevelTimerRunning = false;
            Debug.Log($"Level duration: {levelDuration} seconds");

            // Если длительность меньше 15 секунд или дольше 15 минут, не отправлять статистику
            if (levelDuration < 15f || levelDuration > 900f)
            {
                Debug.Log("Level duration out of bounds, analytics not sent.");
                return;
            }

            // Формируем JSON-данные для события level_completed_stats
            string levelData = $@"
    {{
        ""level_completed_stats"": {{
            ""{levelName}"": {{
                ""duration"": {levelDuration},
                ""num_collectables"": {num_collectables},
                ""status"": ""{status}""
            }}
        }}
    }}";

            AppMetrica.ReportEvent("level_completed_stats", levelData);
            Debug.Log($"Level completed stats event sent for {levelName}");
        }

        private void PauseLevelTimer()
        {
            if (!isLevelTimerRunning)
                return;

            pausedTime = Time.time;
            Debug.Log("Level timer paused");
        }

        private void ResumeLevelTimer()
        {
            if (!isLevelTimerRunning)
                return;

            float pauseDuration = Time.time - pausedTime;
            levelStartTime += pauseDuration;
            Debug.Log("Level timer resumed");
        }

        // Методы для управления сбором FPS
        private void StartFPSTracking()
        {
            if (isFPSTracking)
                return;

            InvokeRepeating(nameof(CaptureFPS), 0f, 1f); // Сбор FPS каждую секунду
            isFPSTracking = true;
            Debug.Log("FPS tracking started");
        }

        private void EndFPSTracking()
        {
            if (!isFPSTracking)
                return;

            StopFPSTracking();
            SendFPSData();
            isFPSTracking = false;
            Debug.Log("FPS tracking ended");
        }

        private void PauseFPSTracking()
        {
            if (!isFPSTracking)
                return;

            CancelInvoke(nameof(CaptureFPS));
            Debug.Log("FPS tracking paused");
        }

        private void ResumeFPSTracking()
        {
            if (!isFPSTracking)
                return;

            InvokeRepeating(nameof(CaptureFPS), 0f, 1f);
            Debug.Log("FPS tracking resumed");
        }

        private void StopFPSTracking()
        {
            CancelInvoke(nameof(CaptureFPS));
        }

        private void CaptureFPS()
        {
            float currentFPS = 1.0f / Time.deltaTime;
            fpsList.Add(currentFPS);
        }

        private void SendFPSData()
        {
            // Проверяем длительность уровня
            float levelDuration = Time.time - levelStartTime;

            // Если длительность меньше 15 секунд или дольше 15 минут, не отправлять статистику
            if (levelDuration < 15f || levelDuration > 900f)
            {
                Debug.Log("Level duration out of bounds, FPS analytics not sent.");
                return;
            }

            // Обработка данных FPS - отрезаем по 2.5% лучших и худших значений
            fpsList.Sort();
            int count = fpsList.Count;

            if (count == 0)
            {
                Debug.Log("No FPS data collected.");
                return;
            }

            int cutOff = Mathf.FloorToInt(count * 0.025f);
            List<float> trimmedFPSList = fpsList.Skip(cutOff).Take(count - 2 * cutOff).ToList();

            // Вычисление среднего/медианного/минимального/максимального FPS
            float averageFPS = trimmedFPSList.Average();
            float medianFPS = trimmedFPSList[trimmedFPSList.Count / 2];
            float minFPS = trimmedFPSList.Min();
            float maxFPS = trimmedFPSList.Max();

            // Формируем JSON-данные для события level_fps_stats
            string jsonData = $@"
    {{
        ""level_fps_stats"": {{
            ""{levelName}"": {{
                ""average_fps"": ""{averageFPS:F2}"",
                ""median_fps"": ""{medianFPS:F2}"",
                ""max_fps"": ""{maxFPS:F2}"",
                ""min_fps"": ""{minFPS:F2}""
            }}
        }}
    }}";

            AppMetrica.ReportEvent("level_fps_stats", jsonData);
            Debug.Log($"FPS stats event sent for {levelName}");
        }

        // Обновление статуса уровня
        public void SetStatus(int collected = 0, int max_collectables_on_lvl = 2)
        {
            // Если за прохождение уровня собрал всё, то статус "full", иначе "default"
            status = collected == max_collectables_on_lvl ? "full" : "default";
        }

        // Обновление количества собранных коллектаблов
        public void UpdateCollectables(int collected)
        {
            num_collectables = collected;
        }

        // Метод для начала сбора аналитики
        public void StartLevelAnalytics()
        {
            StartLevelTimer();
            StartFPSTracking();
            SendLevelStartedEvent(); // Отправляем событие level_started
        }
    }
}
