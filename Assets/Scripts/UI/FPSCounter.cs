using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class FPSCounter : MonoBehaviour
{
    private TMP_Text fpsText;
    private List<float> lastSecondFpsValues = new List<float>(); // Значения FPS за последнюю секунду
    private List<float> fiveSecondFpsValues = new List<float>(); // Значения FPS за последние 5 секунд для мин, макс, среднего

    private float oneSecondTimer = 1.0f; // Таймер для обновления текущего FPS
    private float fiveSecondTimer = 5.0f; // Таймер для обновления статистических данных

    private float minFps = float.MaxValue;
    private float maxFps = 0.0f;
    private float avgFps = 0.0f;

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
        float deltaTime = Time.unscaledDeltaTime;
        float fps = 1.0f / deltaTime;
        lastSecondFpsValues.Add(fps);
        fiveSecondFpsValues.Add(fps);

        oneSecondTimer -= deltaTime;
        fiveSecondTimer -= deltaTime;

        // Обновление текущего FPS каждую секунду
        if (oneSecondTimer <= 0)
        {
            float averageCurrentFps = lastSecondFpsValues.Average();
            lastSecondFpsValues.Clear();
            oneSecondTimer = 1.0f;

            fpsText.text = $"FPS: {Mathf.Ceil(averageCurrentFps)}";
            fpsText.text += $"/nMin: {Mathf.Ceil(minFps)}, Max: {Mathf.Ceil(maxFps)}, Avg: {Mathf.Ceil(avgFps)}";
        }

        // Обновление статистических данных каждые 5 секунд
        if (fiveSecondTimer <= 0)
        {
            if (fiveSecondFpsValues.Any())
            {
                List<float> filteredFps = FilterOutliers(fiveSecondFpsValues);
                minFps = filteredFps.Min();
                maxFps = filteredFps.Max();
                avgFps = filteredFps.Average();
            }
            fiveSecondFpsValues.Clear();
            fiveSecondTimer = 5.0f;
        }
    }

    List<float> FilterOutliers(List<float> data)
    {
        if (data.Count < 20)
            return data;

        List<float> sortedData = new List<float>(data);
        sortedData.Sort();

        int count = sortedData.Count;
        int lowerIndex = (int)(count * 0.05); // 5% снизу
        int upperIndex = (int)(count * 0.95); // 5% сверху

        return sortedData.GetRange(lowerIndex, upperIndex - lowerIndex);
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
