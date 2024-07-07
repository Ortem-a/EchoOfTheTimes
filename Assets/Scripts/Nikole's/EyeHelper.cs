using UnityEngine;

public class EyeHelper : MonoBehaviour
{
    Animator m_Animator;
    public bool tapped = false;
    private EyeButtonManager manager;

    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        manager = FindObjectOfType<EyeButtonManager>();
        if (manager != null)
        {
            manager.RegisterButton(this);
        }
    }

    public void OnTap()
    {
        if (tapped) return;  // Если кнопка уже активна, ничего не делаем

        tapped = true;
        m_Animator.SetBool("Tap", tapped);

        if (manager != null)
        {
            manager.OnButtonTapped(this);
        }
    }

    public void SetTapped(bool state)
    {
        tapped = state;
        m_Animator.SetBool("Tap", tapped);
    }
}
