#if UNITY_EDITOR
using UnityEngine;

namespace EchoOfTheTimes.CustomSnapping
{
    public class CustomSnapMenu : MonoBehaviour
    {
        [UnityEditor.MenuItem("Custom Snap/Delete All Points")]
        private static void DeleteAllPoints()
        {
            var points = GameObject.FindObjectsOfType<CustomSnapPoint>();

            foreach (var point in points) 
            {
                DestroyImmediate(point.gameObject);
            }
        }
    }
}
#endif