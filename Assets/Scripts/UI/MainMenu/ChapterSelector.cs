using UnityEngine;
using UnityEngine.UI;

public class ChapterSelector : MonoBehaviour
{
    [Header("������ �����")]
    public int chapterIndex;

    [Header("����� ���������� ����")]
    public int totalChapters = 8;

    [Header("������ �� Scrollbar")]
    public Scrollbar scrollbar;

    public void SelectChapter()
    {
        chapterIndex = Mathf.Clamp(chapterIndex, 0, totalChapters - 1);

        float value = (float)chapterIndex / (totalChapters - 1);

        scrollbar.value = value;
    }
}
