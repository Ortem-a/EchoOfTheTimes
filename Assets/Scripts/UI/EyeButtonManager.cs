using System.Collections.Generic;
using UnityEngine;

public class EyeButtonManager : MonoBehaviour
{
    public List<EyeHelper> eyeButtons;

    public void RegisterButton(EyeHelper eyeButton)
    {
        if (!eyeButtons.Contains(eyeButton))
        {
            eyeButtons.Add(eyeButton);
        }
    }

    public void OnButtonTapped(EyeHelper tappedButton)
    {
        foreach (var button in eyeButtons)
        {
            if (button != tappedButton)
            {
                button.SetTapped(false);
            }
        }
    }
}
