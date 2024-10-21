using EchoOfTheTimes.Collectables;
using TMPro;
using UnityEngine;

namespace EchoOfTheTimes.UI
{
    public class PlayerProgressHudView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _progressLabel;

        private int _totalCollectables;

        private void Awake()
        {
            _totalCollectables = FindObjectOfType<CollectableSpawner>().NumberOfPlaceholders;

            UpdateProgress(0);
        }

        public void UpdateProgress(int collected)
        {
            _progressLabel.text = $"{collected} / {_totalCollectables}";
        }
    }
}