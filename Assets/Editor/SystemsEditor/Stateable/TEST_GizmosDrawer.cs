using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GizmoColorPalette", menuName = "Gizmo/Color Palette")]
public class GizmoColorPalette : ScriptableObject
{
    [System.Serializable]
    public struct ColorMapping
    {
        public string id;
        public Color color;
    }

    public List<ColorMapping> colorMappings = new List<ColorMapping>();
    public Color defaultColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    private Dictionary<string, Color> _colorDict;

    public void InitializeDictionary()
    {
        _colorDict = new Dictionary<string, Color>();
        foreach (var mapping in colorMappings)
        {
            if (!string.IsNullOrEmpty(mapping.id) 
            {
                _colorDict[mapping.id] = mapping.color;
            }
        }
    }

    public Color GetColor(string id)
    {
        if (_colorDict == null) InitializeDictionary();
        
        if (string.IsNullOrEmpty(id)) return defaultColor;
        if (_colorDict.TryGetValue(id, out Color color)) return color;
        
        return defaultColor;
    }
}

public static class GizmosUtils
{
    private static GizmoColorPalette _palette;
    private static bool _initialized = false;

    private static void Initialize()
    {
        if (_initialized) return;
        
        _palette = Resources.Load<GizmoColorPalette>("GizmoColorPalette");
        if (_palette == null)
        {
            Debug.LogWarning("GizmoColorPalette not found in Resources. Using default color mapping.");
            // Создаем временную палитру по умолчанию
            _palette = ScriptableObject.CreateInstance<GizmoColorPalette>();
        }
        
        _initialized = true;
    }

    public static Color GetColorFromID(string id)
    {
        Initialize();
        return _palette.GetColor(id);
    }

    public static void DrawHierarchyRecursive(Transform current, Matrix4x4 parentMatrix)
    {
        // Вычисляем мировую матрицу для текущего объекта
        Matrix4x4 localMatrix = Matrix4x4.TRS(
            current.localPosition,
            current.localRotation,
            current.localScale
        );
        
        Matrix4x4 matrix = parentMatrix * localMatrix;
        
        // Отрисовываем меш если он есть
        MeshFilter meshFilter = current.GetComponent<MeshFilter>();
        if (meshFilter != null && meshFilter.sharedMesh != null)
        {
            Gizmos.DrawMesh(
                meshFilter.sharedMesh,
                matrix.GetPosition(),
                matrix.rotation,
                matrix.lossyScale
            );
        }

        // Рекурсивно обрабатываем потомков
        foreach (Transform child in current)
        {
            DrawHierarchyRecursive(child, matrix);
        }
    }
}


#if UNITY_EDITOR
[UnityEditor.InitializeOnLoadMethod]
private static void CreateDefaultPaletteIfNeeded()
{
    if (Resources.Load<GizmoColorPalette>("GizmoColorPalette") != null) return;
    
    var palette = ScriptableObject.CreateInstance<GizmoColorPalette>();
    if (!System.IO.Directory.Exists("Assets/Resources"))
    {
        System.IO.Directory.CreateDirectory("Assets/Resources");
    }
    UnityEditor.AssetDatabase.CreateAsset(palette, "Assets/Resources/GizmoColorPalette.asset");
    UnityEditor.AssetDatabase.SaveAssets();
    Debug.Log("Created default GizmoColorPalette in Resources folder");
}
#endif


#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;

public static class GraphVisibilityGizmosDrawer
{
#if UNITY_EDITOR
    [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
    private static void DrawAllStateablesGizmos(GraphVisibility graph, GizmoType gizmoType)
    {
        Stateable[] stateables = graph.GetComponentsInChildren<Stateable>();
        
        foreach (Stateable stateable in stateables)
        {
            if (stateable.stateOptions == null) continue;
            
            foreach (StateOption option in stateable.stateOptions)
            {
                if (string.IsNullOrEmpty(option.id)) continue;
                
                Gizmos.color = GizmosUtils.GetColorFromID(option.id);
                
                Matrix4x4 matrix = Matrix4x4.TRS(
                    option.position,
                    option.rotation,
                    option.scale
                );
                
                GizmosUtils.DrawHierarchyRecursive(
                    stateable.transform, 
                    matrix
                );
            }
        }
    }
#endif
}


#if UNITY_EDITOR
using UnityEditor;
#endif

public static class StateOptionGizmosDrawer
{
#if UNITY_EDITOR
    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    private static void DrawStateableGizmos(Stateable stateable, GizmoType gizmoType)
    {
        if (stateable == null || stateable.stateOptions == null) return;

        foreach (StateOption option in stateable.stateOptions)
        {
            if (string.IsNullOrEmpty(option.id)) continue;
            
            Gizmos.color = GizmosUtils.GetColorFromID(option.id);
            
            Matrix4x4 matrix = Matrix4x4.TRS(
                option.position,
                option.rotation,
                option.scale
            );
            
            GizmosUtils.DrawHierarchyRecursive(
                stateable.transform, 
                matrix
            );
        }
    }
#endif
}