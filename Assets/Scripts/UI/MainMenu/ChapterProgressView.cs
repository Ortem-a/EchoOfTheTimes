using EchoOfTheTimes.SceneManagement;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace EchoOfTheTimes.UI.MainMenu
{
    public class ChapterProgressView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _progressPerChapterLabel;

        public void UpdateLabel(List<GameLevel> levelsData)
        {
            int collectedInChapter = 0;
            int totalInChapter = 0;

            foreach (GameLevel level in levelsData) 
            {
                collectedInChapter += level.Collected;
                totalInChapter += level.TotalCollectables;
            }

            _progressPerChapterLabel.text = $"{collectedInChapter} / {totalInChapter}";
        }
    }
}