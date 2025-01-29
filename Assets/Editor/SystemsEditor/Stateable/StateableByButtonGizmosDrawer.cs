using Systems.Leveling;
using UnityEditor;
using UnityEngine;

namespace SystemsEditor
{
    public class StateableByButtonGizmosDrawer
    {
        private const string _pathToSettingsAsset = @"Assets/Editor/SystemsEditor/StateGizmosColorSettings.asset";

        public static StateGizmosColorSettingsScriptableObject StateGizmosColorSettings
        {
            get
            {
                if (_stateGizmosColorSettings == null)
                {
                    _stateGizmosColorSettings = AssetDatabase.LoadAssetAtPath<StateGizmosColorSettingsScriptableObject>
                        (_pathToSettingsAsset);

                    if (_stateGizmosColorSettings == null)
                    {
                        Debug.LogError($"There is no setttings .asset file by '{_pathToSettingsAsset}'!");
                    }
                }

                return _stateGizmosColorSettings;
            }
        }

        private static StateGizmosColorSettingsScriptableObject _stateGizmosColorSettings;

        [DrawGizmo(GizmoType.Pickable | GizmoType.Selected | GizmoType.InSelectionHierarchy | GizmoType.InSelectionHierarchy)]
        private static void DrawGizmos(StateableByButton target, GizmoType gizmoType)
        {
            var filters = target.GetComponentsInChildren<MeshFilter>();

            if (filters == null || filters.Length == 0) return;

            if (target.States != null)
            {
                foreach (var item in target.States.Items)
                {
                    Gizmos.color = StateGizmosColorSettings.GetColor(item.Key);

                    var option = item.Value;

#warning нрдекэмн ярнъыхе нрдекэмн ялнрперэ, ю ху дереи блеяре...бхдхлн...

                    GizmosDrawerHelper.DrawWireMeshesByTRS(filters, target.transform, option);
                }
            }
        }
    }
}