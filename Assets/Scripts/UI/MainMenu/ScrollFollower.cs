using UnityEngine;
using UnityEngine.UI;

public class ScrollFollower : MonoBehaviour
{
    public ScrollRect targetScrollRect; // ScrollRect, за которым нужно следовать
    private ScrollRect followerScrollRect; // Текущий ScrollRect, который следует за другим

    private void Awake()
    {
        followerScrollRect = GetComponent<ScrollRect>();

        // Отключаем возможность пользовательского ввода для синхронизируемого ScrollRect
        followerScrollRect.horizontal = false;
        followerScrollRect.vertical = false;
    }

    private void Start()
    {
        if (targetScrollRect != null)
        {
            targetScrollRect.onValueChanged.AddListener(OnTargetScrollChanged);
        }
    }

    private void OnTargetScrollChanged(Vector2 position)
    {
        if (followerScrollRect != null)
        {
            followerScrollRect.horizontalNormalizedPosition = targetScrollRect.horizontalNormalizedPosition;
        }
    }

    private void OnDestroy()
    {
        if (targetScrollRect != null)
        {
            targetScrollRect.onValueChanged.RemoveListener(OnTargetScrollChanged);
        }
    }
}
