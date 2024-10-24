using EchoOfTheTimes.SceneManagement;
using EchoOfTheTimes.UI.MainMenu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.UI.MainMenu
{
    [RequireComponent(typeof(ChapterProgressView))]
    public class PerChapterProgressCalculator : MonoBehaviour
    {
        private List<GameLevel> _levelsData;

        private ChapterProgressView _chapterProgressView;

        [SerializeField]
        private string _chapterTitle;

        [Inject]
        private void Construct(UiMainMenuService mainMenuService)
        {
            _chapterProgressView = GetComponent<ChapterProgressView>();

            _levelsData = mainMenuService.PersistenceService.GetData()
                .Find((chapter) => chapter.Title == _chapterTitle).Levels;

            _chapterProgressView.UpdateLabel(_levelsData);
        }
    }
}