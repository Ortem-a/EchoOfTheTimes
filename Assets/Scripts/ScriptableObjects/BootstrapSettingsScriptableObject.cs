using EchoOfTheTimes.Persistence;
using UnityEngine;

namespace EchoOfTheTimes.ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/BootstrapSettings", order = 6)]
    public class BootstrapSettingsScriptableObject : ScriptableObject
    {
        [field: SerializeField]
        public PresetType UsedSavingPreset { get; set; }
    }
}