using EchoOfTheTimes.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace EchoOfTheTimes.UI.MainMenu
{
    public class ChapterButtonView : MonoBehaviour
    {
        private Image _childImage;

        private void Awake()
        {
            // Берем Image нулевого дочернего элемента
            _childImage = transform.GetChild(0).GetComponent<Image>();
        }

        public void UpdateChapterStatus(StatusType status)
        {
            // Устанавливаем цвет для нулевого ребенка
            switch (status)
            {
                case StatusType.Locked:
                    _childImage.color = Color.gray;
                    break;
                case StatusType.Unlocked:
                    _childImage.color = Color.white;
                    break;
                case StatusType.Completed:
                    // _childImage.color = Color.yellow;
                    break;
            }
        }
    }
}
