using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenTap : MonoBehaviour
{
    public Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Tap() // вообще - при нажатии на штуку
    {
        anim.enabled = true;
    }
    public void End() // в конце анимации
    {
        anim.enabled = false;
    }
}
