using UnityEngine;
using UnityEngine.UI;

public class RadioButtonsController : MonoBehaviour
{
    public Button[] buttons;
    private Animator[] animators;
    private Button activeButton;

    private void Start()
    {
        // �������������� ������ ����������
        animators = new Animator[buttons.Length];

        // ������������� �� ������� ������� ������ � �������� ���������
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.AddListener(() => OnButtonClicked(buttons[i]));
            animators[i] = buttons[i].GetComponent<Animator>();
        }

        // ���������� ������ ������ �� ���������
        SetActiveButton(buttons[0]);
    }

    private void OnButtonClicked(Button clickedButton)
    {
        // ���������, ���� ������ ��� �������, �� ������ �� ������
        if (clickedButton == activeButton) return;

        // ���������� ������� ������ � ������������ ���������
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

        // ��������� ������� �������� ������
        activeButton = newActiveButton;
    }
}
