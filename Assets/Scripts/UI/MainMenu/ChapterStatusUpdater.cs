using EchoOfTheTimes.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.UI.MainMenu
{
    public class ChapterStatusUpdater : MonoBehaviour
    {
        private List<StatusType> _chaptersStatuses = new List<StatusType>();

        [Inject]
        private void Construct(UiMainMenuService mainMenuService)
        {
            var chaptersData = mainMenuService.PersistenceService.GetData();

            foreach (var chapter in chaptersData)
            {
                _chaptersStatuses.Add(chapter.ChapterStatus);
            }
        }

        private void Start()
        {
            var chapterItems = GetComponentsInChildren<ChapterItemClickHandler>();

            for (int i = 0; i < chapterItems.Length; i++)
            {
                chapterItems[i].SetStatus(_chaptersStatuses[i + 1]);
                chapterItems[i].GetComponent<ChapterButtonView>().UpdateChapterStatus(_chaptersStatuses[i + 1]);
            }
        }
    }
}