using TMPro;
using UnityEngine;

namespace EchoOfTheTimes.UI.MainMenu
{
    public class ChapterLockView : MonoBehaviour
    {
        private TMP_Text _chapterLockLabel;

        private void Awake()
        {
            _chapterLockLabel = transform.GetChild(1).GetComponent<TMP_Text>();
        }

        public void UpdateLabel(int progress, int required)
        {
            // Накопительный прогресс / накопительная сумма 
            _chapterLockLabel.text = $"{progress}/{required}";
        }

        public void Unlock()
        {
            gameObject.SetActive(false);
        }
    }
}