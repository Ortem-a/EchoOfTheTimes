using UnityEngine;
using UnityEngine.EventSystems;

public class ChapterItemClickHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // Обработка нажатия на главу
        Debug.Log("Глава нажата: " + gameObject.name);

        // Всякая хуйня после нажатия
    }
}
