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

        private int[] _progressPerChapter;
        private int[] _requiredPerChapter;

        [Inject]
        private void Construct(UiMainMenuService mainMenuService)
        {
            _persistenceService = mainMenuService.PersistenceService;

            var chaptersData = _persistenceService.GetData();

            _progressPerChapter = new int[chaptersData.Count];
            _requiredPerChapter = new int[chaptersData.Count];

            int cumulativeRequired = 0;
            int cumulativeProgress = 0;

            for (int i = 0; i < chaptersData.Count; i++)
            {
                _chaptersStatuses.Add(chaptersData[i].ChapterStatus);

                foreach (var level in chaptersData[i].Levels)
                {
                    _progressPerChapter[i] += level.Collected;
                    _requiredPerChapter[i] += level.TotalCollectables;
                }

                _requiredPerChapter[i] += cumulativeRequired;
                _progressPerChapter[i] += cumulativeProgress;

                cumulativeRequired = _requiredPerChapter[i];
                cumulativeProgress = _progressPerChapter[i];
            }
        }

        private void Start()
        {
            _chapterItems = GetComponentsInChildren<ChapterItemClickHandler>();
            _chaptorSelectorItems = GetComponentsInChildren<ChapterSelector>();

            for (int i = 0; i < _chapterItems.Length; i++)
            {
                _chapterItems[i].SetStatus(_chaptersStatuses[i + 1]);
                _chapterItems[i].SetProgress(_progressPerChapter[i], _requiredPerChapter[i]);
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