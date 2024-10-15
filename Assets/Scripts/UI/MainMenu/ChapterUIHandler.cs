using EchoOfTheTimes.Persistence;
using EchoOfTheTimes.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace EchoOfTheTimes.UI.MainMenu
{
    [RequireComponent(typeof(Image))]
    public class ChapterUIHandler : MonoBehaviour
    {
        public string chapterName; // Название главы, соответствующее этому объекту UI

        private PersistenceService _persistenceService;
        private GameChapter _chapterData; // Данные о конкретной главе из сохранения
        private Image _chapterImage; // Компонент Image для изменения цвета

        private void Awake()
        {
            // Получаем компонент Image, чтобы потом изменять цвет главы
            _chapterImage = GetComponent<Image>();
        }

        private void Start()
        {
            // Получаем PersistenceService для доступа к данным сохранения
            _persistenceService = FindObjectOfType<PersistenceService>();

            // Загружаем данные о главе из сохранения
            _chapterData = GetChapterData();

            // Если данные о главе найдены, выполняем действия в зависимости от статуса главы
            if (_chapterData != null)
            {
                HandleChapterStatus(_chapterData.ChapterStatus);
            }
            else
            {
                Debug.LogError($"Глава с названием {chapterName} не найдена в сохранении.");
            }
        }

        private GameChapter GetChapterData()
        {
            // Получаем список всех глав из сохранения
            var chapters = _persistenceService.GetData();

            // Ищем главу по названию
            foreach (var chapter in chapters)
            {
                if (chapter.Title == chapterName)
                {
                    return chapter;
                }
            }

            // Если глава не найдена, возвращаем null
            return null;
        }

        private void HandleChapterStatus(StatusType status)
        {
            switch (status)
            {
                case StatusType.Locked:
                    // Красный цвет для заблокированной главы
                    SetChapterColor(Color.red);
                    break;
                case StatusType.Unlocked:
                    // Белый цвет для разблокированной главы
                    SetChapterColor(Color.white);
                    break;
                case StatusType.Completed:
                    // Желтый цвет для пройденной главы
                    SetChapterColor(Color.yellow);
                    break;
                default:
                    Debug.LogError($"Неизвестный статус главы {chapterName}: {status}");
                    break;
            }
        }

        private void SetChapterColor(Color color) => _chapterImage.color = color;
    }
}