using EchoOfTheTimes.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EchoOfTheTimes.UI.MainMenu
{
    [RequireComponent(typeof(LevelLockView))]
    public class LevelButtonView : MonoBehaviour
    {
        private Image _childImage;
        private TMP_Text _collectablesLabel;
        private GameObject _grandChild; // Нулевой ребенок нулевого ребенка

        private LevelLockView _levelLockView;

        private void Awake()
        {
            // Берем Image у нулевого ребенка
            _childImage = transform.GetChild(0).GetComponent<Image>();
            _levelLockView = GetComponent<LevelLockView>();

            // Берем TMP_Text, который находится глубже в иерархии детей
            _collectablesLabel = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>();

            // Получаем нулевого ребенка у нулевого ребенка
            _grandChild = transform.GetChild(0).GetChild(0).gameObject;
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
                    _childImage.color = Color.gray;
                    _levelLockView.Lock();
                    _grandChild.SetActive(false);
                    break;
                case StatusType.Unlocked:
                    _childImage.color = Color.white;
                    _levelLockView.Unlock();
                    _grandChild.SetActive(true); 
                    break;
                case StatusType.Completed:
                    _levelLockView.Unlock();
                    _grandChild.SetActive(true);
                    break;
            }
        }
    }
}
