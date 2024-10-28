using UnityEngine;
using UnityEngine.UI;

public class ChapterSelector : MonoBehaviour
{
    [Header("»ндекс главы")]
    public int chapterIndex;

    [Header("ќбщее количество глав")]
    public int totalChapters = 8;

    [Header("—сылка на Scrollbar")]
    public Scrollbar scrollbar;

    public void SelectChapter()
    {
        chapterIndex = Mathf.Clamp(chapterIndex, 0, totalChapters - 1);

        float value = (float)chapterIndex / (totalChapters - 1);

        scrollbar.value = value;
    }
}
