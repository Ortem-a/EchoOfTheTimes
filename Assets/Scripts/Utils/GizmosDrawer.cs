using EchoOfTheTimes.ScriptableObjects;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace EchoOfTheTimes.Utils
{
    public abstract class GizmosDrawer<T> : MonoBehaviour
        where T : Component
    {
#if UNITY_EDITOR
        protected ColorStateSettingsScriptableObject colorStateSettings;
        protected T component;
        protected Mesh mesh;
        protected List<(Mesh m, Transform t)> meshes;

        private void OnDrawGizmosSelected()
        {
            if (component != null && colorStateSettings != null)
            {
                DrawStates();
            }
            else
            {
                InitComponents();
            }
        }

        protected void InitComponents()
        {
            component = GetComponent<T>();

            if (TryGetComponent(out MeshFilter mf))
            {
                mesh = mf.sharedMesh;
            }
            else
            {
                meshes = new List<(Mesh m, Transform t)>();
                var filters = GetComponentsInChildren<MeshFilter>();

                foreach (var filter in filters)
                {
                    meshes.Add((filter.sharedMesh, filter.transform));
                }
            }

            colorStateSettings = AssetDatabase.LoadAssetAtPath<ColorStateSettingsScriptableObject>(
                @"Assets/ScriptableObjects/ColorStateSettings.asset");
        }

        protected abstract void DrawStates();
#endif
    }
}
