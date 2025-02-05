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

        public string levelName = "Default Level";
        public string chapterName = "Default Chapter";
        private string status = "full"; // Теперь всегда "full"
        private int num_collectables = -1; // Всегда отправляем -1

        private List<float> fpsList = new List<float>();

        private void Start()
        {
            StartLevelAnalytics();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            isPaused = pauseStatus;
            if (isPaused)
            {
                if (isLevelTimerRunning) PauseLevelTimer();
                if (isFPSTracking) PauseFPSTracking();
            }
            else
            {
                if (isLevelTimerRunning) ResumeLevelTimer();
                if (isFPSTracking) ResumeFPSTracking();
            }
        }

        public void StartLevelAnalytics()
        {
            StartLevelTimer();
            StartFPSTracking();
            SendLevelStartedEvent(levelName);
        }

        private void SendLevelStartedEvent(string levelName)
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

        private void StartLevelTimer()
        {
            if (isLevelTimerRunning) return;

            levelStartTime = Time.time;
            isLevelTimerRunning = true;
            Debug.Log("Level timer started");
        }

        public void EndLevelAnalytics()
        {
            EndLevelTimer();
            EndFPSTracking();
        }

        private void EndLevelTimer()
        {
            if (!isLevelTimerRunning) return;

            float levelDuration = Time.time - levelStartTime;
            isLevelTimerRunning = false;
            Debug.Log($"Level duration: {levelDuration} seconds");

            if (levelDuration < 15f || levelDuration > 900f)
            {
                Debug.Log("Level duration out of bounds, analytics not sent.");
                return;
            }

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
            if (!isLevelTimerRunning) return;

            pausedTime = Time.time;
            Debug.Log("Level timer paused");
        }

        private void ResumeLevelTimer()
        {
            if (!isLevelTimerRunning) return;

            float pauseDuration = Time.time - pausedTime;
            levelStartTime += pauseDuration;
            Debug.Log("Level timer resumed");
        }

        private void StartFPSTracking()
        {
            if (isFPSTracking) return;

            InvokeRepeating(nameof(CaptureFPS), 0f, 1f);
            isFPSTracking = true;
            Debug.Log("FPS tracking started");
        }

        private void EndFPSTracking()
        {
            if (!isFPSTracking) return;

            StopFPSTracking();
            SendFPSData();
            isFPSTracking = false;
            Debug.Log("FPS tracking ended");
        }

        private void PauseFPSTracking()
        {
            if (!isFPSTracking) return;

            CancelInvoke(nameof(CaptureFPS));
            Debug.Log("FPS tracking paused");
        }

        private void ResumeFPSTracking()
        {
            if (!isFPSTracking) return;

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
            float levelDuration = Time.time - levelStartTime;

            if (levelDuration < 15f || levelDuration > 900f)
            {
                Debug.Log("Level duration out of bounds, FPS analytics not sent.");
                return;
            }

            fpsList.Sort();
            int count = fpsList.Count;

            if (count == 0)
            {
                Debug.Log("No FPS data collected.");
                return;
            }

            int cutOff = Mathf.FloorToInt(count * 0.025f);
            List<float> trimmedFPSList = fpsList.Skip(cutOff).Take(count - 2 * cutOff).ToList();

            float averageFPS = trimmedFPSList.Average();
            float medianFPS = trimmedFPSList[trimmedFPSList.Count / 2];
            float minFPS = trimmedFPSList.Min();
            float maxFPS = trimmedFPSList.Max();

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

        // Убрали проверку коллектаблов, теперь просто задаём "full"
        public void SetStatus()
        {
            status = "full";
        }
    }
}
