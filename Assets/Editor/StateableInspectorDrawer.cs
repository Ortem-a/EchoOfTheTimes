using Systems.Leveling;
using Systems.Tools;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Systems.Leveling.Stateable))]
public class StateableInspectorDrawer : Editor
{
    private int _stateIdToSet;
    private int _stateIdToTransform;

    public override void OnInspectorGUI()
    {
        IStateable stateable = (IStateable)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();

        _stateIdToSet = EditorGUILayout.IntField("State Id", _stateIdToSet);

        if (GUILayout.Button("Set Or Update Params To State"))
        {
            stateable.SetOptionsFrom(_stateIdToSet, Selection.activeTransform);
        }

        EditorGUILayout.Space();

        _stateIdToTransform = EditorGUILayout.IntField("State Id", _stateIdToTransform);

        if (GUILayout.Button("Transform Object By State"))
        {
            if (stateable.TryGetOption(_stateIdToTransform, out var option))
            {
                var selectedObject = Selection.activeTransform;

                selectedObject.SetLocalPositionAndRotation(
                    option.LocalPosition, option.LocalRotation);
                selectedObject.localScale = option.LocalScale;
            }
        }

        EditorGUILayout.Space();

        GUILayout.TextArea(stateable.OptionsToString());
    }
}