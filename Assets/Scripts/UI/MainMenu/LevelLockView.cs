using UnityEngine;

namespace EchoOfTheTimes.UI.MainMenu
{
    public class LevelLockView : MonoBehaviour
    {
        private GameObject _lockIcon;

        private void Awake()
        {
            _lockIcon = transform.GetChild(1).gameObject;
        }

        public void Lock()
        {
            _lockIcon.SetActive(true);
        }

        public void Unlock()
        {
            _lockIcon.SetActive(false);
        }
    }
}