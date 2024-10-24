using EchoOfTheTimes.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EchoOfTheTimes.UI.MainMenu
{
    [RequireComponent(typeof(Image))]
    public class LevelButtonView : MonoBehaviour
    {
        private Image _image;
        private TMP_Text _collectablesLabel;

        private void Awake()
        {
            _image = GetComponent<Image>();

            _collectablesLabel = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        }

        public void UpdateData(GameLevel levelData)
        {
            UpdateLevelStatus(levelData.LevelStatus);

            UpdateLabel(levelData.Collected, levelData.TotalCollectables);
        }

        private void UpdateLabel(int collected, int total)
        {
            _collectablesLabel.text = $"{collected}/{total}";
        }

        private void UpdateLevelStatus(StatusType status)
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
                    // _image.color = Color.yellow;
                    break;
            }
        }
    }
}