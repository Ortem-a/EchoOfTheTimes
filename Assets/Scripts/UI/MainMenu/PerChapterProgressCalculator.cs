using EchoOfTheTimes.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.UI.MainMenu
{
    [RequireComponent(typeof(ChapterProgressView))]
    public class PerChapterProgressCalculator : MonoBehaviour
    {
        [SerializeField]
        private string _chapterTitle;

        [Inject]
        private void Construct(UiMainMenuService mainMenuService)
        {
            var levelsData = mainMenuService.PersistenceService.GetData()
                .Find((chapter) => chapter.Title == _chapterTitle).Levels;

            GetComponent<ChapterProgressView>().UpdateLabel(levelsData);
        }
    }
}