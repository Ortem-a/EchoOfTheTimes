using UnityEditor;
using UnityEngine;

namespace SystemsEditor
{
    public static class GizmosUtils
    {
        private static StateGizmosColorSettingsScriptableObject _palette;
        private static bool _initialized = false;

        private static void Initialize()
        {
            if (_initialized) return;

            _palette = AssetDatabase.LoadAssetAtPath<StateGizmosColorSettingsScriptableObject>(
                @"Assets/Editor/SystemsEditor/StateGizmosColorSettings.asset");

            _initialized = true;
        }

        public static Color GetGizmoColorByState(int stateId)
        {
            Initialize();
            return _palette.GetColor(stateId);
        }
    }
}