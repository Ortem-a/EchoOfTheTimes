using System;
using UnityEngine;

[Serializable]
public class SceneField
{
#if UNITY_EDITOR
    [SerializeField] private UnityEditor.SceneAsset sceneAsset;
#endif
    [SerializeField] private string sceneName;

    public string SceneName => sceneName;

#if UNITY_EDITOR
    public UnityEditor.SceneAsset SceneAsset
    {
        get => sceneAsset;
        set
        {
            sceneAsset = value;
            sceneName = sceneAsset != null ? sceneAsset.name : string.Empty;
        }
    }
#endif
}
