using EchoOfTheTimes.Persistence;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.UI.MainMenu
{
    public class ColumnService : MonoBehaviour
    {
        private Column[] _columns;
        private Column _activeColumn;

        private PersistenceService _persistenceService;

        private void Awake()
        {
            _persistenceService = FindObjectOfType<PersistenceService>();
            _columns = GetComponentsInChildren<Column>();
        }

        private void Start()
        {
            for (int i = 0; i < _columns.Length; i++)
            {
                _columns[i].Id = i;

                //var gameChapter = PersistenceService.SaveLoadService.DataToSave.Data[i + 1];
                var gameChapter = _persistenceService.GetData()[i + 1];

                _columns[i].Chapter = gameChapter;

                _columns[i].Initialize();

                _columns[i].MarkAs(gameChapter.ChapterStatus);

                for (int j = 0; j < gameChapter.Levels.Count; j++)
                {
                    _columns[i].Segments[j].MarkAs(gameChapter.Levels[j].LevelStatus);
                }
            }

            //var firstLockedChapterIndex = PersistenceService.SaveLoadService.DataToSave.Data
            var firstLockedChapterIndex = _persistenceService.GetData()
                .FindIndex((x) => x.ChapterStatus == SceneManagement.StatusType.Locked);

            if (firstLockedChapterIndex == -1)
            {
                //firstLockedChapterIndex = PersistenceService.SaveLoadService.DataToSave.Data.Count;
                firstLockedChapterIndex = _persistenceService.GetData().Count;
            }

            _activeColumn = _columns[firstLockedChapterIndex - 2];

            _activeColumn.SetEnable(true);
            _activeColumn.Raise();
        }

        public void HandleTouch(int index)
        {
            for (int i = 0; i < _columns.Length; i++)
            {
                _columns[i].SetEnable(false);
            }

            _activeColumn.Fall(() =>
            {
                for (int i = 0; i < _columns.Length; i++)
                {
                    _columns[i].SetEnable(true);
                }
            });

            _activeColumn = _columns[index];
        }
    }
}