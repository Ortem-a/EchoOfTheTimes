using EchoOfTheTimes.Persistence;
using EchoOfTheTimes.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.UI.MainMenu
{
    public class ChapterStatusUpdater : MonoBehaviour
    {
        private List<StatusType> _chaptersStatuses = new List<StatusType>();

        private ChapterItemClickHandler[] _chapterItems;
        private ChapterSelector[] _chaptorSelectorItems;
        private PersistenceService _persistenceService;

        [Inject]
        private void Construct(UiMainMenuService mainMenuService)
        {
            _persistenceService = mainMenuService.PersistenceService;
            var chaptersData = _persistenceService.GetData();

            // «аполн€ем список статусов глав
            for (int i = 0; i < chaptersData.Count; i++)
            {
                _chaptersStatuses.Add(chaptersData[i].ChapterStatus);
            }
        }

        private void Start()
        {
            _chapterItems = GetComponentsInChildren<ChapterItemClickHandler>();
            _chaptorSelectorItems = GetComponentsInChildren<ChapterSelector>();

            Debug.LogWarning("«аглушка на всего 4 главы дл€ главного меню");

            // ќбновл€ем только статус главы, без учета прогресса коллектаблов
            for (int i = 0; i < 4; i++)
            {
                _chapterItems[i].SetStatus(_chaptersStatuses[i + 1]);
                // ѕередаем 0, так как прогресс коллектаблов более не отслеживаетс€
                _chapterItems[i].SetProgress(0, 0);
                _chapterItems[i].GetComponent<ChapterButtonView>().UpdateChapterStatus(_chaptersStatuses[i + 1]);
            }
        }

        public ChapterItemClickHandler GetChapterItem(string levelFullName)
        {
            var chapterName = levelFullName.Split('|')[0];
            var data = _persistenceService.GetData();
            var index = data.FindIndex((chapter) => chapter.Title == chapterName);
            return _chapterItems[index - 1];
        }

        public ChapterSelector GetChapterSelectorItem(string levelFullName)
        {
            var chapterName = levelFullName.Split('|')[0];
            var data = _persistenceService.GetData();
            var index = data.FindIndex((chapter) => chapter.Title == chapterName);
            return _chaptorSelectorItems[index - 1];
        }
    }
}
