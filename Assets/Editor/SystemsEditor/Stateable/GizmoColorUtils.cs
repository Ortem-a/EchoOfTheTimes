using UnityEditor;
using UnityEngine;

namespace SystemsEditor
{
    public static class GizmoColorUtils
    {
        private static StateGizmosColorSettingsScriptableObject _stateGizmosPalette;
        private static VertexGizmosColorSettingsScriptableObject _vertexGizmosPalette;
        private static bool _statesInitialized = false;
        private static bool _verticesInitialized = false;

        private static void InitializeStates()
        {
            if (_statesInitialized) return;

            _stateGizmosPalette = AssetDatabase.LoadAssetAtPath<StateGizmosColorSettingsScriptableObject>(
                @"Assets/Editor/SystemsEditor/StateGizmosColorSettings.asset");

            _statesInitialized = true;
        }

        private static void InitializeVertices()
        {
            if (_verticesInitialized) return;

            _vertexGizmosPalette = AssetDatabase.LoadAssetAtPath<VertexGizmosColorSettingsScriptableObject>(
                @"Assets/Editor/SystemsEditor/VertexGizmosColorSettings.asset");

            _verticesInitialized = true;
        }

        public static Color GetGizmoColorByState(int stateId)
        {
            InitializeStates();
            return _stateGizmosPalette.GetColor(stateId);
        }

        public static Color32 StaticColor()
        {
            InitializeVertices();
            return _vertexGizmosPalette.Static;
        }

        public static Color32 MovingColor()
        {
            InitializeVertices();
            return _vertexGizmosPalette.Moving;
        }

        public static Color32 BridgeColor()
        {
            InitializeVertices();
            return _vertexGizmosPalette.Bridge;
        }

        public static Color32 ConnectionColor()
        {
            InitializeVertices();
            return _vertexGizmosPalette.Connection;
        }

        public static Color32 ArrowColor()
        {
            InitializeVertices();
            return _vertexGizmosPalette.Arrow;
        }

        public static Color32 TextColor()
        {
            InitializeVertices();
            return _vertexGizmosPalette.Text;
        }
    }
}