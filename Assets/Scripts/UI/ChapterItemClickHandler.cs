using UnityEngine;
using UnityEngine.EventSystems;

public class ChapterItemClickHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // ��������� ������� �� �����
        Debug.Log("����� ������: " + gameObject.name);

        // ������ ����� ����� �������
    }
}
