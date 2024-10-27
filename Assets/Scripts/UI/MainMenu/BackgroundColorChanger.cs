using UnityEngine;
using UnityEngine.UI;

public class BackgroundColorChanger : MonoBehaviour
{
    [Header("Список цветов для каждой главы")]
    public Color[] chapterColors = new Color[8];

    [Header("Ссылка на Scrollbar")]
    public Scrollbar scrollbar;

    private Image panelImage;

    void Start()
    {
        panelImage = GetComponent<Image>();
    }

    void Update()
    {
        float value = scrollbar.value; // Значение от 0 до 1

        int chaptersCount = chapterColors.Length;

        // Рассчитываем индекс текущего интервала
        float scaledValue = value * (chaptersCount - 1);
        int index = Mathf.FloorToInt(scaledValue);

        // Ограничиваем индекс в пределах массива
        index = Mathf.Clamp(index, 0, chaptersCount - 2);

        // Рассчитываем коэффициент интерполяции t
        float t = scaledValue - index;

        // Получаем цвета для интерполяции
        Color fromColor = chapterColors[index];
        Color toColor = chapterColors[index + 1];

        // Выполняем интерполяцию цвета
        Color interpolatedColor = Color.Lerp(fromColor, toColor, t);

        // Устанавливаем цвет панели
        panelImage.color = interpolatedColor;
    }
}
