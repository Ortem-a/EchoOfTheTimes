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

        ChapterItemClickHandler[] chapterItems;
        PersistenceService _persistenceService;

        [Inject]
        private void Construct(UiMainMenuService mainMenuService)
        {
            _persistenceService = mainMenuService.PersistenceService;

            var chaptersData = mainMenuService.PersistenceService.GetData();

            foreach (var chapter in chaptersData)
            {
                _chaptersStatuses.Add(chapter.ChapterStatus);
            }
        }

        private void Start()
        {
            //var chapterItems = GetComponentsInChildren<ChapterItemClickHandler>();
            chapterItems = GetComponentsInChildren<ChapterItemClickHandler>();

            for (int i = 0; i < chapterItems.Length; i++)
            {
                chapterItems[i].SetStatus(_chaptersStatuses[i + 1]);
                chapterItems[i].GetComponent<ChapterButtonView>().UpdateChapterStatus(_chaptersStatuses[i + 1]);
            }
        }

        public ChapterItemClickHandler GetChapterItem(string levelFullName)
        {
            var chapterName = levelFullName.Split('|')[0];

            var data = _persistenceService.GetData();

            var index = data.FindIndex((chapter) => chapter.Title == chapterName);

            return chapterItems[index - 1];
        }
    }
}