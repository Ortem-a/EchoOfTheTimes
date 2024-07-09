using UnityEngine;
using UnityEngine.UI;

public class RadioButtonsController : MonoBehaviour
{
    public Button[] buttons;
    private Animator[] animators;
    private Button activeButton;

    private void Start()
    {
        // Инициализируем массив аниматоров
        animators = new Animator[buttons.Length];

        // Подписываемся на события нажатия кнопок и получаем аниматоры
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.AddListener(() => OnButtonClicked(buttons[i]));
            animators[i] = buttons[i].GetComponent<Animator>();
        }

        // Активируем первую кнопку по умолчанию
        SetActiveButton(buttons[0]);
    }

    private void OnButtonClicked(Button clickedButton)
    {
        // Проверяем, если кнопка уже активна, то ничего не делаем
        if (clickedButton == activeButton) return;

        // Активируем нажатую кнопку и деактивируем остальные
        SetActiveButton(clickedButton);
    }

    private void SetActiveButton(Button newActiveButton)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] == newActiveButton)
            {
                animators[i].SetBool("IsActive", true);
                animators[i].Play("Eye_ToOpen");
            }
            else
            {
                animators[i].SetBool("IsActive", false);
                animators[i].Play("Eye_ToClosed");
            }
        }

        // Обновляем текущую активную кнопку
        activeButton = newActiveButton;
    }
}
