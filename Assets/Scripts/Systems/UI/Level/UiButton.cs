using UnityEngine;
using UnityEngine.UI;

namespace Systems.UI.Level
{
    [RequireComponent(typeof(Button))]
    public abstract class UiButton : MonoBehaviour
    {
        protected Button button;

        protected virtual void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(HandleButtonClicked);
        }

        protected abstract void HandleButtonClicked();
    }
}