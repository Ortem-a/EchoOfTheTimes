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
        public string levelName = "Default Level"; // Название уровня
        public string chapterName = "Default Chapter"; // Название главы
        private string status = "default"; // "default" - прошёл без сбора всех коллектаблов, "full" - собрал всё
        private int num_collectables = 0; // Количество собранных коллектаблов за это прохождение уровня

        // FPS
        private List<float> fpsList = new List<float>();

        private void Start()
        {
            // Запуск таймера уровня и сбор данных FPS
            StartLevelTimer();
            StartFPSTracking();
        }

        private void OnDisable()
        {
            // Остановка таймера уровня и отправка аналитики
            EndLevelTimer();
            EndFPSTracking();
        }

        //-------------------------------------------------------------------

        private void StartLevelTimer()
        {
            levelStartTime = Time.time;
            Debug.Log("Level timer started");
        }

        private void EndLevelTimer()
        {
            float levelDuration = Time.time - levelStartTime;
            Debug.Log($"Level duration: {levelDuration} seconds");

            // AppMetrica - статистики уровня
            string jsonData = $"{{ \"level_name\": \"{levelName}\"," +
                              $" \"chapter_name\": \"{chapterName}\"," +
                              $" \"duration\": \"{levelDuration}\"," +
                              $" \"num_collectables\": \"{num_collectables.ToString()}\"," +
                              $" \"status\": \"{status}\" }}";

            AppMetrica.ReportEvent("level_completed", jsonData);

            // MyTracker - статистики уровня
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

        private void StartFPSTracking()
        {
            InvokeRepeating(nameof(CaptureFPS), 0f, 1f); // Сбор FPS каждую секунду
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
            // Обработка данных FPS - отрезаем по 2.5% лучших и худших ибо нехуй
            fpsList.Sort();
            int count = fpsList.Count;
            int cutOff = Mathf.FloorToInt(count * 0.025f);
            List<float> trimmedFPSList = fpsList.Skip(cutOff).Take(count - 2 * cutOff).ToList();

            // Вычисление среднего/медианного/минимального/максимального FPS
            float averageFPS = trimmedFPSList.Average();
            float medianFPS = trimmedFPSList[trimmedFPSList.Count / 2];
            float minFPS = trimmedFPSList.Min();
            float maxFPS = trimmedFPSList.Max();

            // AppMetrica - статистики производительности
            string jsonData = $"{{ \"level_name\": \"{levelName}\"," +
                              $" \"chapter_name\": \"{chapterName}\"," +
                              $" \"average_fps\": \"{averageFPS:F2}\"," +
                              $" \"median_fps\": \"{medianFPS:F2}\"," +
                              $" \"max_fps\": \"{maxFPS:F2}\"," +
                              $" \"min_fps\": \"{minFPS:F2}\" }}";

            AppMetrica.ReportEvent("level_fps_stats", jsonData);

            // MyTracker - статистики производительности
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

        // Надо как-то прокинуть сюда данные
        public void SetStatus(int collected, int max_collectables_on_lvl)
        {
            if (collected < 0 || max_collectables_on_lvl < 0) return;

            // Если за прохождение уровня собрал всё, то красава, иначе дефолтыч
            if (collected == max_collectables_on_lvl)
            {
                status = "full";
            }
            else
            {
                status = "full";
            }
        }

        // Пупупу
        public void UpdateCollectables(int collected)
        {
            num_collectables = collected;
        }
    }
}
