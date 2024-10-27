using UnityEngine;
using UnityEngine.UI;

public class PanelPositionController : MonoBehaviour
{
    [Header("Панели для управления")]
    public RectTransform[] panels; // Назначьте ваши панели здесь

    [Header("Ссылка на Scrollbar")]
    public Scrollbar scrollbar; // Ссылка на ваш Scrollbar

    [Header("Ссылка на канвас")]
    public RectTransform canvasRectTransform; // Ссылка на RectTransform канваса

    private int panelCount;
    private float shiftDistance;

    void Start()
    {
        panelCount = panels.Length;

        if (panelCount < 2)
        {
            Debug.LogError("Необходимо указать как минимум 2 панели.");
            return;
        }

        if (canvasRectTransform == null)
        {
            Debug.LogError("Необходимо назначить RectTransform канваса.");
            return;
        }

        // Получаем высоту канваса для shiftDistance
        shiftDistance = canvasRectTransform.rect.height;

        // Инициализируем позиции панелей
        for (int i = 0; i < panelCount; i++)
        {
            if (i == 0)
            {
                // Первая панель смещена вниз
                panels[i].anchoredPosition = new Vector2(0, -shiftDistance);
            }
            else
            {
                // Остальные панели на позиции (0, 0)
                panels[i].anchoredPosition = Vector2.zero;
            }
        }
    }

    void Update()
    {
        // Проверяем наличие Scrollbar
        if (scrollbar == null)
        {
            Debug.LogError("Необходимо назначить Scrollbar.");
            return;
        }

        // Получаем значение Scrollbar
        float value = scrollbar.value; // Значение от 0 до 1

        // Рассчитываем масштабированное значение и индексы
        float scaledValue = value * (panelCount - 1);
        int index = Mathf.FloorToInt(scaledValue);
        index = Mathf.Clamp(index, 0, panelCount - 2);
        float t = scaledValue - index;

        // Позиции
        Vector2 inPosition = Vector2.zero;
        Vector2 outPosition = new Vector2(0, -shiftDistance);

        // Перемещаем панели с нелинейным переходом
        for (int i = 0; i < panelCount; i++)
        {
            if (i == index)
            {
                // Текущая панель
                if (t <= 0.5f)
                {
                    // Первая фаза: панель поднимается вверх
                    float phaseT = t / 0.5f; // Нормализуем t от 0 до 0.5 в 0 до 1
                    panels[i].anchoredPosition = Vector2.Lerp(outPosition, inPosition, phaseT);
                }
                else
                {
                    // Вторая фаза: панель остается на месте
                    panels[i].anchoredPosition = inPosition;
                }
            }
            else if (i == index + 1)
            {
                // Следующая панель
                if (t > 0.5f)
                {
                    // Вторая фаза: панель опускается вниз
                    float phaseT = (t - 0.5f) / 0.5f; // Нормализуем t от 0.5 до 1 в 0 до 1
                    panels[i].anchoredPosition = Vector2.Lerp(inPosition, outPosition, phaseT);
                }
                else
                {
                    // Первая фаза: панель остается на месте
                    panels[i].anchoredPosition = inPosition;
                }
            }
            else
            {
                // Остальные панели остаются на позиции (0, 0)
                panels[i].anchoredPosition = inPosition;
            }
        }
    }
}
