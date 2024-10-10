using EchoOfTheTimes.Persistence;
using EchoOfTheTimes.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace EchoOfTheTimes.UI.MainMenu
{
    [RequireComponent(typeof(Image))]
    public class LevelUIHandler : MonoBehaviour
    {
        public string chapterName; // �������� �����, � ������� ��������� �������
        public string levelName;   // �������� ������

        private PersistenceService _persistenceService;
        private GameLevel _levelData; // ������ � ���������� ������
        private Image _levelImage;    // ��������� Image ��� ��������� �����

        private void Awake()
        {
            // �������� ��������� Image, ����� ����� �������� ���� ������
            _levelImage = GetComponent<Image>();
        }

        private void Start()
        {
            // �������� PersistenceService ��� ������� � ������ ����������
            _persistenceService = FindObjectOfType<PersistenceService>();

            // ��������� ������ �� ������ �� ����������
            _levelData = GetLevelData();

            // ���� ������ �� ������ �������, ��������� �������� � ����������� �� ������� ������
            if (_levelData != null)
            {
                HandleLevelStatus(_levelData.LevelStatus);
            }
            else
            {
                Debug.LogError($"������� � ��������� {levelName} � ����� {chapterName} �� ������ � ����������.");
            }
        }

        private GameLevel GetLevelData()
        {
            // �������� ��� ����� �� ����������
            var chapters = _persistenceService.GetData();

            // ���� ������ ����� �� ��������
            foreach (var chapter in chapters)
            {
                if (chapter.Title == chapterName)
                {
                    // ���� ������ ������� � ��������� �����
                    foreach (var level in chapter.Levels)
                    {
                        if (level.LevelName == levelName)
                        {
                            return level; // ���������� ��������� �������
                        }
                    }
                }
            }

            // ���� ������� �� ������, ���������� null
            return null;
        }

        private void HandleLevelStatus(StatusType status)
        {
            switch (status)
            {
                case StatusType.Locked:
                    // ������� ���� ��� ���������������� ������
                    SetLevelColor(Color.red);
                    break;
                case StatusType.Unlocked:
                    // ����� ���� ��� ����������������� ������
                    SetLevelColor(Color.white);
                    break;
                case StatusType.Completed:
                    // ������ ���� ��� ����������� ������
                    SetLevelColor(Color.yellow);
                    break;
                default:
                    Debug.LogError($"����������� ������ ������ {levelName} � ����� {chapterName}: {status}");
                    break;
            }
        }

        private void SetLevelColor(Color color) => _levelImage.color = color;
    }
}