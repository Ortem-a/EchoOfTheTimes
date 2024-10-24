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
            _lockIcon.gameObject.SetActive(true);
        }

        public void Unlock()
        {
            _lockIcon.gameObject.SetActive(false);
        }
    }
}