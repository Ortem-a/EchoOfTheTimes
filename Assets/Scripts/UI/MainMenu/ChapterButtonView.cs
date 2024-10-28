using EchoOfTheTimes.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace EchoOfTheTimes.UI.MainMenu
{
    public class ChapterButtonView : MonoBehaviour
    {
        private Image _childImage;

        [SerializeField] private GameObject additionalObjectToDisable;
        [SerializeField] private GameObject anotherObjectToGrayOut;
        [SerializeField] private Image directObjectToGrayOut;

        private void Awake()
        {
            _childImage = transform.GetChild(0).GetComponent<Image>();
        }

        public void UpdateChapterStatus(StatusType status)
        {
            switch (status)
            {
                case StatusType.Locked:
                    _childImage.color = Color.gray;
                    additionalObjectToDisable.transform.GetChild(0).gameObject.SetActive(false);
                    anotherObjectToGrayOut.transform.GetChild(0).GetComponent<Image>().color = Color.gray;
                    directObjectToGrayOut.color = Color.gray;
                    break;
                case StatusType.Unlocked:
                    _childImage.color = Color.white;
                    additionalObjectToDisable.transform.GetChild(0).gameObject.SetActive(true);
                    anotherObjectToGrayOut.transform.GetChild(0).GetComponent<Image>().color = Color.white;
                    directObjectToGrayOut.color = Color.white;
                    break;
                case StatusType.Completed:
                    additionalObjectToDisable.transform.GetChild(0).gameObject.SetActive(true);
                    anotherObjectToGrayOut.transform.GetChild(0).GetComponent<Image>().color = Color.white;
                    directObjectToGrayOut.color = new Color(0.7f, 1f, 0.7f);
                    break;
            }
        }
    }
}
