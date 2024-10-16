using EchoOfTheTimes.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace EchoOfTheTimes.UI.MainMenu
{
    public class ChapterButtonView : MonoBehaviour
    {
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void UpdateChapterStatus(StatusType status)
        {
            switch (status)
            {
                case StatusType.Locked:
                    _image.color = Color.red;
                    break;
                case StatusType.Unlocked:
                    _image.color = Color.white;
                    break;
                case StatusType.Completed:
                    _image.color = Color.yellow;
                    break;
            }
        }
    }
}