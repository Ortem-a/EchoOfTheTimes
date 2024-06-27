using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeHelper : MonoBehaviour
{
    Animator m_Animator;
    public bool tapped = false;

    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
    }

    public void OnTap()
    {
        tapped = !tapped;
        m_Animator.SetBool("Tap", tapped);
    }
}
