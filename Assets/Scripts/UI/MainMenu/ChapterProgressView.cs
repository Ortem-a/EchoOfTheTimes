using System.Collections.Generic;
using TMPro;
using UnityEngine;
using EchoOfTheTimes.SceneManagement;

namespace EchoOfTheTimes.UI.MainMenu
{
    public class ChapterProgressView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _progressPerChapterLabel;

        public void UpdateLabel(List<GameLevel> levelsData)
        {
            // ѕрогресс коллектаблов больше не отображаетс€
            _progressPerChapterLabel.text = "";
        }
    }
}
