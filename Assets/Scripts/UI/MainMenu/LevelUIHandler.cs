using EchoOfTheTimes.Persistence;
using EchoOfTheTimes.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace EchoOfTheTimes.UI.MainMenu
{
    [RequireComponent(typeof(Image))]
    public class LevelUIHandler : MonoBehaviour
    {
        public string chapterName; // Название главы, в которой находится уровень
        public string levelName;   // Название уровня

        private PersistenceService _persistenceService;
        private GameLevel _levelData; // Данные о конкретном уровне
        private Image _levelImage;    // Компонент Image для изменения цвета

        private void Awake()
        {
            // Получаем компонент Image, чтобы потом изменять цвет уровня
            _levelImage = GetComponent<Image>();
        }

        private void Start()
        {
            // Получаем PersistenceService для доступа к данным сохранения
            _persistenceService = FindObjectOfType<PersistenceService>();

            // Загружаем данные об уровне из сохранения
            _levelData = GetLevelData();

            // Если данные об уровне найдены, выполняем действия в зависимости от статуса уровня
            if (_levelData != null)
            {
                HandleLevelStatus(_levelData.LevelStatus);
            }
            else
            {
                Debug.LogError($"Уровень с названием {levelName} в главе {chapterName} не найден в сохранении.");
            }
        }

        private GameLevel GetLevelData()
        {
            // Получаем все главы из сохранений
            var chapters = _persistenceService.GetData();

            // Ищем нужную главу по названию
            foreach (var chapter in chapters)
            {
                if (chapter.Title == chapterName)
                {
                    // Ищем нужный уровень в найденной главе
                    foreach (var level in chapter.Levels)
                    {
                        if (level.LevelName == levelName)
                        {
                            return level; // Возвращаем найденный уровень
                        }
                    }
                }
            }

            // Если уровень не найден, возвращаем null
            return null;
        }

        private void HandleLevelStatus(StatusType status)
        {
            switch (status)
            {
                case StatusType.Locked:
                    // Красный цвет для заблокированного уровня
                    SetLevelColor(Color.red);
                    break;
                case StatusType.Unlocked:
                    // Белый цвет для разблокированного уровня
                    SetLevelColor(Color.white);
                    break;
                case StatusType.Completed:
                    // Желтый цвет для пройденного уровня
                    SetLevelColor(Color.yellow);
                    break;
                default:
                    Debug.LogError($"Неизвестный статус уровня {levelName} в главе {chapterName}: {status}");
                    break;
            }
        }

        private void SetLevelColor(Color color) => _levelImage.color = color;
    }
}