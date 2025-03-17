using Systems.Leveling;
using UnityEditor;
using UnityEngine;

namespace SystemsEditor
{
    public class StairsCreatorMenuItem
    {
        [MenuItem("GameObject/Prefabs/Stairs Creator", false, 2)]
        private static void InstantiatePrefab()
        {
            var stairsCreator = AssetDatabase.LoadAssetAtPath<StairsCreator>(
                "Assets/_MEGAGIGAREFACTOR/Stairs/StairsCreator.prefab");

            PrefabUtility.InstantiatePrefab(stairsCreator);
        }
    }
}