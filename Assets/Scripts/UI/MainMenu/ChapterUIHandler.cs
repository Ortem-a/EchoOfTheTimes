using EchoOfTheTimes.Persistence;
using EchoOfTheTimes.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace EchoOfTheTimes.UI.MainMenu
{
    [RequireComponent(typeof(Image))]
    public class ChapterUIHandler : MonoBehaviour
    {
        public string chapterName; // �������� �����, ��������������� ����� ������� UI

        private PersistenceService _persistenceService;
        private GameChapter _chapterData; // ������ � ���������� ����� �� ����������
        private Image _chapterImage; // ��������� Image ��� ��������� �����

        private void Awake()
        {
            // �������� ��������� Image, ����� ����� �������� ���� �����
            _chapterImage = GetComponent<Image>();
        }

        private void Start()
        {
            // �������� PersistenceService ��� ������� � ������ ����������
            _persistenceService = FindObjectOfType<PersistenceService>();

            // ��������� ������ � ����� �� ����������
            _chapterData = GetChapterData();

            // ���� ������ � ����� �������, ��������� �������� � ����������� �� ������� �����
            if (_chapterData != null)
            {
                HandleChapterStatus(_chapterData.ChapterStatus);
            }
            else
            {
                Debug.LogError($"����� � ��������� {chapterName} �� ������� � ����������.");
            }
        }

        private GameChapter GetChapterData()
        {
            // �������� ������ ���� ���� �� ����������
            var chapters = _persistenceService.GetData();

            // ���� ����� �� ��������
            foreach (var chapter in chapters)
            {
                if (chapter.Title == chapterName)
                {
                    return chapter;
                }
            }

            // ���� ����� �� �������, ���������� null
            return null;
        }

        private void HandleChapterStatus(StatusType status)
        {
            switch (status)
            {
                case StatusType.Locked:
                    // ������� ���� ��� ��������������� �����
                    SetChapterColor(Color.red);
                    break;
                case StatusType.Unlocked:
                    // ����� ���� ��� ���������������� �����
                    SetChapterColor(Color.white);
                    break;
                case StatusType.Completed:
                    // ������ ���� ��� ���������� �����
                    SetChapterColor(Color.yellow);
                    break;
                default:
                    Debug.LogError($"����������� ������ ����� {chapterName}: {status}");
                    break;
            }
        }

        private void SetChapterColor(Color color) => _chapterImage.color = color;
    }
}