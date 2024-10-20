using UnityEngine;
using Io.AppMetrica;
using Mycom.Tracker.Unity;
using System.Collections.Generic;
using System;
using System.Linq;

namespace EchoOfTheTimes.SceneManagement
{
    public class LevelAnalyticsTracker : MonoBehaviour
    {
        private float levelStartTime;
        public string levelName = "Default Level"; // �������� ������
        public string chapterName = "Default Chapter"; // �������� �����
        private string status = "default"; // "default" - ������ ��� ����� ���� ������������, "full" - ������ ��
        private int num_collectables = 0; // ���������� ��������� ������������ �� ��� ����������� ������

        private List<float> fpsList = new List<float>(); // ������ FPS���

        private void Start()
        {
            // ������ ������� ������ � ���� ������ FPS
            StartLevelTimer();
            StartFPSTracking();
        }

        private void OnDisable()
        {
            // ��������� ������� ������ � �������� ���������
            EndLevelTimer();
            EndFPSTracking();
        }

        private void StartLevelTimer()
        {
            levelStartTime = Time.time;
            Debug.Log("Level timer started");
        }

        private void EndLevelTimer()
        {
            float levelDuration = Time.time - levelStartTime;
            Debug.Log($"Level duration: {levelDuration} seconds");

            // AppMetrica - ���������� ������
            string levelData = $@"
            {{
                ""level_name"": ""{levelName}"",
                ""details"": {{
                    ""duration"": {levelDuration},
                    ""num_collectables"": {num_collectables},
                    ""status"": ""{status}""
                }}
            }}";

            AppMetrica.ReportEvent("level_completed", levelData);

            // MyTracker - ���������� ������
            if (Application.platform == RuntimePlatform.Android)
            {
                var eventCustomParams = new Dictionary<string, string>
                {
                    ["level_name"] = levelName,
                    ["chapter_name"] = chapterName,
                    ["duration"] = levelDuration.ToString(),
                    ["num_collectables"] = num_collectables.ToString(),
                    ["status"] = status
                };

                MyTracker.TrackEvent("level_completed", eventCustomParams);
            }
        }

        private void StartFPSTracking()
        {
            InvokeRepeating(nameof(CaptureFPS), 0f, 1f); // ���� FPS ������ �������
        }

        private void EndFPSTracking()
        {
            StopFPSTracking();
            SendFPSData();
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
            // ��������� ������ FPS - �������� �� 2.5% ������ � ������ ��� �����
            fpsList.Sort();
            int count = fpsList.Count;
            int cutOff = Mathf.FloorToInt(count * 0.025f);
            List<float> trimmedFPSList = fpsList.Skip(cutOff).Take(count - 2 * cutOff).ToList();

            // ���������� ��������/����������/������������/������������� FPS
            float averageFPS = trimmedFPSList.Average();
            float medianFPS = trimmedFPSList[trimmedFPSList.Count / 2];
            float minFPS = trimmedFPSList.Min();
            float maxFPS = trimmedFPSList.Max();

            // AppMetrica - ���������� ������������������
            string levelData = $@"
            {{
                ""level_name"": ""{levelName}"",
                ""details"": {{
                    ""average_fps"": ""{averageFPS:F2}"",
                    ""median_fps"": ""{medianFPS:F2}"",
                    ""max_fps"": ""{maxFPS:F2}"",
                    ""min_fps"": ""{minFPS:F2}""
                }}
            }}";

            AppMetrica.ReportEvent("level_fps_stats", levelData);


            // MyTracker - ���������� ������������������
            if (Application.platform == RuntimePlatform.Android)
            {
                var eventCustomParams = new Dictionary<string, string>
                {
                    ["level_name"] = levelName,
                    ["chapter_name"] = chapterName,
                    ["average_fps"] = averageFPS.ToString(),
                    ["median_fps"] = medianFPS.ToString(),
                    ["max_fps"] = maxFPS.ToString(),
                    ["min_fps"] = minFPS.ToString()
                };

                MyTracker.TrackEvent("level_fps_stats", eventCustomParams);
            }
        }

        // ���� ���-�� ��������� ���� ������
        public void SetStatus(int collected = 0, int max_collectables_on_lvl = 2)
        {
            if (collected < 0 || max_collectables_on_lvl < 0) return;

            // ���� �� ����������� ������ ������ ��, �� �������, ����� ��������
            if (collected == max_collectables_on_lvl)
            {
                status = "full";
            }
            else
            {
                status = "default";
            }
        }

        // ������
        public void UpdateCollectables(int collected)
        {
            num_collectables = collected;
        }
    }
}
