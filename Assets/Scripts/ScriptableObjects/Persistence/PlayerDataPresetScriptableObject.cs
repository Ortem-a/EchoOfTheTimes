using EchoOfTheTimes.Persistence;
using UnityEngine;

namespace EchoOfTheTimes.ScriptableObjects.Persistence
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Persistence Preset", order = 8)]
    public class PlayerDataPresetScriptableObject : ScriptableObject
    {
        public PlayerData Data;
    }
}