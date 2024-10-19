using System;
using System.Collections.Generic;
using UnityEngine;
using Io.AppMetrica;
using System.Linq;

namespace EchoOfTheTimes.SceneManagement
{
    public class FPSTracker : MonoBehaviour
    {
        private List<float> fpsList = new List<float>();
        public string levelName = "Default Level";  // Название уровня
        public string chapterName = "Default Chapter"; // Название главы

        private void Start()
        {
            // Начинаем собирать FPS при старте уровня
            InvokeRepeating("CaptureFPS", 0f, 1f); // Сбор данных каждые 1 секунду
        }

        private void OnDisable()
        {
            // Останавливаем сбор данных и отправляем метрику при завершении уровня
            CancelInvoke("CaptureFPS");
            SendFPSData();
        }

        private void CaptureFPS()
        {
            float currentFPS = 1.0f / Time.deltaTime;
            fpsList.Add(currentFPS);
        }

        private void SendFPSData()
        {
            if (fpsList.Count == 0)
            {
                Debug.LogWarning("No FPS data collected.");
                return;
            }

            // Сортируем FPS
            fpsList.Sort();

            // Отсекаем 2.5% лучших и худших значений
            int count = fpsList.Count;
            int cutOff = Mathf.FloorToInt(count * 0.025f);  // 2.5% отсечение
            List<float> trimmedFPSList = fpsList.Skip(cutOff).Take(count - 2 * cutOff).ToList();

            // Вычисляем статистику
            float averageFPS = trimmedFPSList.Average();
            float medianFPS = trimmedFPSList[trimmedFPSList.Count / 2];
            float minFPS = trimmedFPSList.Min();
            float maxFPS = trimmedFPSList.Max();

            Debug.Log($"Average FPS: {averageFPS}");
            Debug.Log($"Median FPS: {medianFPS}");
            Debug.Log($"Min FPS: {minFPS}");
            Debug.Log($"Max FPS: {maxFPS}");

            // Формируем данные для отправки в AppMetrica
            string jsonData = "{ \"level_name\": \"" + levelName + "\", " +
                              "\"chapter_name\": \"" + chapterName + "\", " +
                              "\"average_fps\": \"" + averageFPS.ToString("F2") + "\", " +
                              "\"median_fps\": \"" + medianFPS.ToString("F2") + "\", " +
                              "\"max_fps\": \"" + maxFPS.ToString("F2") + "\", " +
                              "\"min_fps\": \"" + minFPS.ToString("F2") + "\" }";

            // Отправляем данные в AppMetrica
            AppMetrica.ReportEvent("level_fps_stats", jsonData);

            Debug.Log("Sent FPS statistics to AppMetrica");
        }
    }
}