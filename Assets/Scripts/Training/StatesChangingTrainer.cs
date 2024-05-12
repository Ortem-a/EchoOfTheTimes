using DG.Tweening;
using UnityEngine;

namespace EchoOfTheTimes.Training
{
    public class StatesChangingTrainer : MechanicTrainer
    {
        [SerializeField]
        private GameObject _infoLabel;
        [SerializeField]
        private GameObject _bottomPanel;

        [SerializeField]
        private Artifact _artifact;

        [SerializeField]
        private float _fadingDuration_sec;

        private void Awake()
        {
            EnableUiPanels(false, 0f, 0f);

            _artifact.gameObject.SetActive(true);

            _artifact.Enable();
        }

        public override void Disable()
        {
            throw new System.NotImplementedException();
        }

        public override void Activate()
        {
            EnableUiPanels(true, 1f, _fadingDuration_sec);
        }

        private void EnableUiPanels(bool isEnable, float toScale, float duration)
        {
            _infoLabel.SetActive(isEnable);
            _infoLabel.transform.DOScale(toScale, duration);

            _bottomPanel.SetActive(isEnable);
            _bottomPanel.transform.DOScale(toScale, duration);
        }
    }
}