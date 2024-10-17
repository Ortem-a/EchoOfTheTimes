using UnityEngine;
using Io.AppMetrica;

namespace EchoOfTheTimes.SceneManagement
{
    public class LevelTimeTracker : MonoBehaviour
    {
        private float levelStartTime;
        public string levelName = "Default Level";  // �������� �� ����������� ��� ������, ���� ���������
        public string chapterName = "Default Chapter"; // �������� �� ����������� ��� �����, ���� ���������

        private void Start()
        {
            StartLevelTimer(); // ��������� ������ ��� ������ ������
        }

        private void OnDisable()
        {
            EndLevel(); // ��������� ������ � ���������� ������� ����� ����������� �����
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

            // ��������� JSON-������ � ��������������� ��������� ��� AppMetrica
            string jsonData = "{ \"level_name\": \"" + levelName + "\", \"chapter_name\": \"" + chapterName + "\", \"duration\": \"" + levelDuration.ToString() + "\" }";

            // ���������� ������ � AppMetrica
            AppMetrica.ReportEvent("level_completed", jsonData);

            Debug.Log("Sent level completion duration to AppMetrica");
        }
    }
}
