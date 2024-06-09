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

    public void Tap() // ������ - ��� ������� �� �����
    {
        anim.enabled = true;
    }
    public void End() // � ����� ��������
    {
        anim.enabled = false;
    }
}
