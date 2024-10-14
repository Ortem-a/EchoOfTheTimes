using EchoOfTheTimes.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EchoOfTheTimes.UI.MainMenu
{
    public class LevelStatusUpdater : MonoBehaviour
    {
        private LevelButtonView[] _views;
        private LevelButtonHandler[] _handlers;
        private List<GameLevel> _levelsData;

        [SerializeField]
        private ChapterProgressView _chapterProgressView;

        [SerializeField]
        private string _chapterTitle;

        [Inject]
        private void Construct(UiMainMenuService mainMenuService)
        {
            _levelsData = mainMenuService.PersistenceService.GetData()
                .Find((chapter) => chapter.Title == _chapterTitle).Levels;

            _chapterProgressView.UpdateLabel(_levelsData);
        }

        private void Awake()
        {
            _views = GetComponentsInChildren<LevelButtonView>();
            _handlers = new LevelButtonHandler[_views.Length];
            for (int i = 0; i < _views.Length; i++)
            {
                _handlers[i] = _views[i].GetComponent<LevelButtonHandler>();
            }
        }

        private void Start()
        {
            UpdateLevelsData();
        }

        private void UpdateLevelsData()
        {
            for (int i = 0; i < _views.Length; i++)
            {
                _views[i].UpdateData(_levelsData[i]);
                _handlers[i].SetData(_levelsData[i]);
            }
        }
    }
}