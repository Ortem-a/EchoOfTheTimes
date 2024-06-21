using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    public string url = "https://www.example.com";

    public void OpenLink()
    {
        Application.OpenURL(url);
    }
}
