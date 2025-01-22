using System.Collections.Generic;
using Systems;
using Systems.Leveling;
using UnityEditor;
using UnityEngine;

namespace SystemsEditor
{
    [CustomEditor(typeof(TestMovableByRules))]
    public class MovableByRulesEditor : UnityEditor.Editor
    {
        private int _ruleIndex;

        public override void OnInspectorGUI()
        {
            TestMovableByRules movableByRules = (TestMovableByRules)target;

            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("Add To Rules"))
            {
                var newRule = new Rule()
                {
                    Option = (StateOption)movableByRules.transform,
                };

                movableByRules.Rules ??= new List<Rule>();

                movableByRules.Rules.Add(newRule);
            }

            EditorGUILayout.Space();

            _ruleIndex = EditorGUILayout.IntField("Rule Index", _ruleIndex);

            EditorGUILayout.Space();

            if (GUILayout.Button("To Rule"))
            {
                if (movableByRules.Rules == null || movableByRules.Rules.Count == 0 || _ruleIndex > movableByRules.Rules.Count - 1)
                {
                    Debug.LogError($"Invalid index: '{_ruleIndex}'!");
                }
                else
                {
                    movableByRules.transform.ApplyStateOption(movableByRules.Rules[_ruleIndex].Option);
                }
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Edit Rule"))
            {
                var newRule = new Rule()
                {
                    Option = (StateOption)movableByRules.transform,
                };

                if (movableByRules.Rules == null || movableByRules.Rules.Count == 0)
                {
                    movableByRules.Rules ??= new List<Rule>() { newRule };
                }
                else if (_ruleIndex > movableByRules.Rules.Count - 1)
                {
                    Debug.LogError($"Invalid index: '{_ruleIndex}'!");
                }
                else
                {
                    movableByRules.Rules[_ruleIndex] = newRule;
                }
            }

            EditorGUILayout.Space();
        }
    }
}