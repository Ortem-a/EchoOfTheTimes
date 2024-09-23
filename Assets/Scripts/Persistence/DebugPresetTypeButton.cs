using EchoOfTheTimes.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace EchoOfTheTimes.Persistence
{
    [RequireComponent(typeof(Button))]
    public class DebugPresetTypeButton : MonoBehaviour
    {
        [SerializeField]
        private PresetType _presetType;

        private Button _button;

        private BootstrapSettingsScriptableObject _bootstrapSettings;

        private void Awake()
        {
            _bootstrapSettings = Resources.Load(@"ScriptableObjects/BootstrapSettings") as BootstrapSettingsScriptableObject;

            _button = GetComponent<Button>();
            _button.onClick.AddListener(HandleButtonClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(HandleButtonClick);
        }

        private void HandleButtonClick()
        {
            Debug.Log($"[{name}] ON CLICK! Preset type: {_bootstrapSettings.UsedSavingPreset} -> {_presetType}");

            _bootstrapSettings.UsedSavingPreset = _presetType;

            SceneManager.LoadScene("Bootstrapper", LoadSceneMode.Single);
        }
    }
}