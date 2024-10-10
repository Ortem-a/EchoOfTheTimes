using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EchoOfTheTimes.UI.MainMenu
{
    [RequireComponent(typeof(Image))]
    public class MuteButtonController : MonoBehaviour
    {
        private Image _image;

        [SerializeField]
        private Sprite _mutedIcon;
        [SerializeField]
        private Sprite _soundEnableIcon;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void SetButtonIcon(bool isMuted)
        {
            if (isMuted)
            {
                _image.sprite = _mutedIcon;
            }
            else
            {
                _image.sprite = _soundEnableIcon;
            }
        }
    }
}