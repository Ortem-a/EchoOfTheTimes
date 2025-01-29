using UnityEngine;

namespace SystemsEditor
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Editor/StateGizmosColorSettings", order = 1)]
    public class StateGizmosColorSettingsScriptableObject : ScriptableObject
    {
        public Color[] StateGizmosColors;

        public Color GetColor(int stateId)
        {
            if (stateId < 0 || stateId >= StateGizmosColors.Length)
            {
                Debug.LogWarning($"There is no specified color for state: '{stateId}'! Used '{Color.magenta}' instead!");
                return Color.magenta;
            }

            return StateGizmosColors[stateId];
        }
    }
}