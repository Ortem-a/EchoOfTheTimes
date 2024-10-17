using UnityEngine;
using Io.AppMetrica;

namespace EchoOfTheTimes.SceneManagement
{
    public class LevelTimeTracker : MonoBehaviour
    {
        private float levelStartTime;
        public string levelName = "Default Level";  // Замените на фактическое имя уровня, если требуется
        public string chapterName = "Default Chapter"; // Замените на фактическое имя главы, если требуется

        private void Start()
        {
            StartLevelTimer(); // Запускаем таймер при старте уровня
        }

        private void OnDisable()
        {
            EndLevel(); // Завершаем таймер и отправляем метрику перед завершением сцены
        }

        private void StartLevelTimer()
        {
            levelStartTime = Time.time;
            Debug.Log("Level timer started");
        }

        private void EndLevel()
        {
            float levelDuration = Time.time - levelStartTime;
            Debug.Log($"Level duration: {levelDuration} seconds");

            // Формируем JSON-строку с экранированными кавычками для AppMetrica
            string jsonData = "{ \"level_name\": \"" + levelName + "\", \"chapter_name\": \"" + chapterName + "\", \"duration\": \"" + levelDuration.ToString() + "\" }";

            // Отправляем данные в AppMetrica
            AppMetrica.ReportEvent("level_completed", jsonData);

            Debug.Log("Sent level completion duration to AppMetrica");
        }
    }
}
