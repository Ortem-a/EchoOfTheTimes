using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Animator))]
public class EyeHelper : MonoBehaviour
{
    private Animator _animator;
    private Button _button;

    private bool _tapped = false;

    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
        _button = gameObject.GetComponent<Button>();

        _button.onClick.AddListener(OnTap);
    }

    private void OnTap()
    {
        _tapped = !_tapped;
        _animator.SetBool("Tap", _tapped);
    }
}
