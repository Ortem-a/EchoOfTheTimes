using EchoOfTheTimes.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EchoOfTheTimes.UI.MainMenu
{
    [RequireComponent(typeof(Image), typeof(LevelLockView))]
    public class LevelButtonView : MonoBehaviour
    {
        private Image _image;
        private TMP_Text _collectablesLabel;

        private LevelLockView _levelLockView;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _levelLockView = GetComponent<LevelLockView>();

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
                    _image.color = new Color(78f, 78f, 78f);
                    _levelLockView.Lock();
                    break;
                case StatusType.Unlocked:
                    _image.color = Color.white;
                    _levelLockView.Unlock();
                    break;
            }
        }
    }
}