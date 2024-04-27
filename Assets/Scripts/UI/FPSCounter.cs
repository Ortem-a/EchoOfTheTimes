using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    private TMP_Text fpsText;
    private float deltaTime = 0.0f;

    void Start()
    {
        Transform fpsDisplayTransform = FindDeepChild(transform.root, "TextFPSCounter");
        if (fpsDisplayTransform != null)
        {
            fpsText = fpsDisplayTransform.GetComponent<TMP_Text>();
        }
        else
        {
            Debug.LogWarning("No 'TextFPSCounter' object found in the hierarchy. Please ensure it is named correctly and has a TMP_Text component.");
        }
    }

    void Update()
    {
        if (fpsText != null)
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            fpsText.text = Mathf.Ceil(fps).ToString() + " FPS";
        }
    }

    Transform FindDeepChild(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;
            Transform found = FindDeepChild(child, name);
            if (found != null)
                return found;
        }
        return null;
    }
}
